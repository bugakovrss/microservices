using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SmartHome.EventlogApi.Configuration;
using SmartHome.EventlogApi.ErrorHandling;
using SmartHome.EventlogApi.WebExtensions;
using SmartHome.Model;
using SmartHome.Model.Entities;
using SmartHome.Model.Initializers;
using Steeltoe.Discovery.Client;

namespace SmartHome.EventlogApi
{
    public class Startup
    {
        private const string ApplicationName = "smarthome-eventlogapi-app";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDiscoveryClient(Configuration);

            services.AddMvcCore().AddResponseFormatters();

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
                    policy.RequireClaim("scope", "eventlogapi");
                });
            });

            services.AddSwaggerEx(ApplicationName);

            services.AddSingleton<IRepository<DeviceEvent>>(ioc => new Repository<DeviceEvent>(
                DeviceEventInitializer.Initialize()));
        }


        public void Configure(IApplicationBuilder app,  IApiVersionDescriptionProvider versionProvider)
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
