using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CodacyChallenge.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var sharedFolder = Path.Combine(hostingContext.HostingEnvironment.ContentRootPath, "..", "CodacyChallenge.Common");

                config.SetBasePath(sharedFolder);
                config.AddJsonFile("config.json",
                       optional: false,
                       reloadOnChange: true);
            })
                .UseStartup<Startup>();
    }
}
