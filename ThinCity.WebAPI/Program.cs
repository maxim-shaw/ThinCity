using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using System.IO;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ThinCity.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Application global error exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((builderContext, config) =>
            {
                IHostingEnvironment env = builderContext.HostingEnvironment;
                config
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            })
            .ConfigureLogging(logging => 
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Information);
            })
            .UseNLog()
            .UseStartup<Startup>();
    }
}
