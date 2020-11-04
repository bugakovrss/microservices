using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.AuthApi.Configuration;
using SmartHome.AuthApi.ErrorHandling;
using SmartHome.AuthApi.Services;
using SmartHome.AuthApi.WebExtensions;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;

namespace SmartHome.AuthApi
{
    public class Startup
    {
        private const string ApplicationName = "smarthome-authapi-app";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDiscoveryClient(Configuration);

            services.AddMvcCore().AddResponseFormatters();

            services.AddSwaggerEx(ApplicationName);

            services.AddScoped<IAuthService, AuthService>();

            services.AddHttpClient("AuthService")
                .AddServiceDiscovery()
                .AddTypedClient<IAuthService, AuthService>();

            services.Configure<IdentityServerSettings>(Configuration.GetSection("IdentityServer"));
        }


        public void Configure(IApplicationBuilder app,  IApiVersionDescriptionProvider versionProvider)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseDiscoveryClient();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUiEx(versionProvider);
        }
    }
}
