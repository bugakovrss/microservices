using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SmartHome.EventlogApi.WebExtensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Конфигурация swagger
        /// </summary>
        /// <param name="serviceCollection">Доступные сервисы</param>
        /// <param name="applicationName">Имя приложения</param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerEx(this IServiceCollection serviceCollection, string applicationName)
        {
            serviceCollection.AddSwaggerInternal(applicationName,
                options =>
                {
                    string basePath = AppContext.BaseDirectory;
                    options.IncludeXmlComments(Path.Combine(basePath, "SmartHome.EventlogApi.xml"));
                    options.IncludeXmlComments(Path.Combine(basePath, "SmartHome.Model.xml"));
                }
            );

            return serviceCollection;
        }

        /// <summary>
        /// Конфигурация Json форматирования
        /// </summary>
        /// <param name="mvcCoreBuilder"></param>
        /// <returns></returns>
        public static IMvcCoreBuilder AddResponseFormatters(this IMvcCoreBuilder mvcCoreBuilder)
        {
            mvcCoreBuilder.AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    opt.SerializerSettings.Formatting = Formatting.Indented;
                    opt.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    opt.SerializerSettings.Converters.Add(new StringEnumConverter(typeof(CamelCaseNamingStrategy)));
                })
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase))); ;

            return mvcCoreBuilder;
        }

        private static IServiceCollection AddSwaggerInternal(
          this IServiceCollection services,
          string applicationName,
          Action<SwaggerGenOptions> configure = null)
        {
            services.AddApiVersioning((Action<ApiVersioningOptions>)(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            }));

            services.AddVersionedApiExplorer((Action<ApiExplorerOptions>)(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            }));

            services.AddSwaggerGen((Action<SwaggerGenOptions>)(options =>
            {
                foreach (ApiVersionDescription versionDescription in (IEnumerable<ApiVersionDescription>)ServiceProviderServiceExtensions.GetRequiredService<IApiVersionDescriptionProvider>(ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services)).ApiVersionDescriptions)
                    options.SwaggerDoc(versionDescription.GroupName, new OpenApiInfo()
                    {
                        Title = applicationName,
                        Version = versionDescription.ApiVersion.ToString()
                    });

                options.UseInlineDefinitionsForEnums();

                options.MapType<Guid>((Func<OpenApiSchema>)(() => new OpenApiSchema()
                {
                    Type = "string",
                    Format = "uuid"
                }));
                options.MapType<Decimal>((Func<OpenApiSchema>)(() => new OpenApiSchema()
                {
                    Type = "number",
                    Format = ""
                }));
                options.MapType<Decimal?>((Func<OpenApiSchema>)(() => new OpenApiSchema()
                {
                    Type = "number",
                    Format = ""
                }));

                Action<SwaggerGenOptions> action = configure;
                if (action == null)
                    return;
                action(options);
            }));

            return services;
        }

        public static IApplicationBuilder UseSwaggerUiEx(
          this IApplicationBuilder builder,
          IApiVersionDescriptionProvider versioningProvider)
        {
            return builder.UseSwaggerUI((Action<SwaggerUIOptions>)(options =>
            {
                foreach (ApiVersionDescription versionDescription in (IEnumerable<ApiVersionDescription>)versioningProvider.ApiVersionDescriptions)
                    options.SwaggerEndpoint("/swagger/" + versionDescription.GroupName + "/swagger.json", versionDescription.GroupName.ToUpperInvariant());
                options.DisplayOperationId();
            }));
        }
    }
}
