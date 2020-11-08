using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
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
        private ILogger<Startup> _logger;
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
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddServiceDiscovery()
                .AddTypedClient<IEventlogService, EventlogService>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy())
                .AddPolicyHandler(GetTimeoutPolicy());
        }

        private  IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .OrResult(msg => msg.StatusCode == HttpStatusCode.InternalServerError)
                .WaitAndRetryAsync(2, attempt =>
                        TimeSpan.FromSeconds(0.1 * Math.Pow(2, attempt)),
                    (ex, duration) =>
                    {
                        _logger?.LogError(
                            $"Error on send query to service. Retry through '{duration.TotalMilliseconds}' ms. '{ex.Result?.StatusCode}':'{ex.Exception?.Message}' ");
                    });
        }

        private IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(6);
        }

        private IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .CircuitBreakerAsync(3,
                    TimeSpan.FromSeconds(20),
                    (ex, breakDelay) => { _logger?.LogError($"CirketBreaker '{breakDelay.TotalMilliseconds}' StatusCode'{ex.Result?.StatusCode}':{ex.Exception?.Message}"); },
                    () =>
                    {
                        _logger.LogInformation(".Breaker logging: Call ok! Closed the circuit again!");
                    },
                    () =>
                    {
                        _logger.LogInformation(".Breaker logging: Half-open: Next call is a trial!");
                    }
                    );
        }

        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider versionProvider)
        {
            _logger = app.ApplicationServices.GetService<ILogger<Startup>>();
            _logger.LogInformation("Запуск....");

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
