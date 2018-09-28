using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog.Web;
using Microsoft.Extensions.Logging;

namespace wSiteMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }            
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())

                                //aca se configuran los puertos que se abriran para recibir peticiones
                                .AddJsonFile("hosting.json", optional: true)
                                .Build();

            var host = WebHost.CreateDefaultBuilder(args)
                                .UseConfiguration(config)
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseStartup<Startup>()
                                .ConfigureLogging(logging =>
                                {
                                    logging.ClearProviders();
                                    logging.SetMinimumLevel(LogLevel.Trace);
                                })
                                .UseNLog()
                                .Build();

            return host;
        }            
    }
}
