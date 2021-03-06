using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SmartHome.AuthApi;

namespace SmartHome.EventlogApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseDefaultServiceProvider(configure => configure.ValidateScopes = false)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
