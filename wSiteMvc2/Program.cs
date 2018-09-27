using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace wSiteMvc2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
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
                                .Build();

            return host;
        }
    }
}
