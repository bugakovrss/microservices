using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SmartHome.ControlApi.Configuration;
using SmartHome.ControlApi.ErrorHandling;
using SmartHome.ControlApi.Services;
using SmartHome.ControlApi.WebExtensions;
using SmartHome.Model;
using SmartHome.Model.Entities;
using SmartHome.Model.Initializers;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;

namespace SmartHome.ControlApi
{
    public class Startup
    {
        private const string ApplicationName = "smarthome-controlapi-app";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDiscoveryClient(Configuration);

            services.AddMvcCore().AddResponseFormatters();

            services.AddHttpContextAccessor();

            var identityServerSettings = new IdentityServerSettings();
            Configuration.Bind("IdentityServer", identityServerSettings);

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = identityServerSettings.Host;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };

                    options.RequireHttpsMetadata = false;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "controlapi");
                });
            });

            services.AddSwaggerEx(ApplicationName);

            services.AddSingleton<IRepository<Device>>(ioc => new Repository<Device>(
                DeviceInitializer.Initialize()));

            services.Configure<EventlogApiEndpoints>(Configuration.GetSection("EventlogApi"));

            services.AddScoped<IEventlogService, EventlogService>();

            // Configure HttpClient
            services.AddHttpClient<HttpClient>("eventlogs")
                .AddServiceDiscovery()
                .AddTypedClient<IEventlogService, EventlogService>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider versionProvider)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseDiscoveryClient();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization("ApiScope");
            });

            app.UseSwagger();
            app.UseSwaggerUiEx(versionProvider);
        }
    }
}
