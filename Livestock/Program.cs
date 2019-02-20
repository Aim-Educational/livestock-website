using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Livestock
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseUrls("https://localhost:7777/", "https://192.168.1.150:7777/")
                   .ConfigureAppConfiguration((ctx, config) =>
                   {
                       config.AddEnvironmentVariables(prefix: "LIVESTOCK_");
                   })
                   .UseStartup<Startup>();
    }
}
