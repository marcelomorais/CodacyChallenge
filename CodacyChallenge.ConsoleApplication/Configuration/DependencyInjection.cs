using CodacyChallenge.Application;
using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Service.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace CodacyChallenge.ConsoleApplication.Configuration
{
    public class DependencyInjection
    {
        public ServiceProvider ServiceBuilder()
        {
            var configuration = ConfigurationBuilder();

            var serviceProvider = new ServiceCollection()
          .AddOptions()
          .Configure<Configuration>(options => configuration.GetSection("Config").Bind(options))
          .AddTransient<IGitEngine, GitCLIEngine>()
          .AddTransient<IStartApplication, StartApplication>()
          .BuildServiceProvider();

            return serviceProvider;
        }
        public IConfigurationRoot ConfigurationBuilder()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("config.json",
                       optional: false,
                       reloadOnChange: true);
            
            IConfigurationRoot configuration = builder.Build();

            return configuration;
        }
    }
}
