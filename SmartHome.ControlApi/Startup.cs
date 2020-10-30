using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.ControlApi.ErrorHandling;
using SmartHome.ControlApi.WebExtensions;
using SmartHome.Model;
using SmartHome.Model.Entities;
using SmartHome.Model.Initializers;

namespace SmartHome.ControlApi
{
    public class Startup
    {
        private const string ApplicationName = "smart-home-control-api";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddResponseFormatters();

            services.AddSwaggerEx(ApplicationName);

            services.AddSingleton<IRepository<Device>>(ioc => new Repository<Device>(DeviceInitializer.Initialize()));
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider versionProvider)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

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
