using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace XrplNftTicketing.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var Logger = NLogBuilder.ConfigureNLog(String.IsNullOrWhiteSpace(environment) ? "nlog.config" : $"nlog.{environment}.config").GetCurrentClassLogger();

            try
            {
                Logger.Trace("Main init running");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "System stopped due to exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                }).UseNLog();
    }
}
