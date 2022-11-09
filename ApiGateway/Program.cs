using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ApiGateway;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.local.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json")
                        .AddEnvironmentVariables();
                })
              .UseStartup<Startup>();
    }
}