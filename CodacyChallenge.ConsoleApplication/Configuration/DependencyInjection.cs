using CodacyChallenge.Application;
using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Service.Client;
using CodacyChallenge.Service.Client.Interface;
using CodacyChallenge.Service.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
          .AddTransient<IGitEngine, GitCLIEngine>()
          .AddTransient<IStartApplication, StartApplication>()
          .AddSingleton<IPowershellWrapper, PowershellWrapper>()
          .AddSingleton<IMemoryCacheWrapper>(x => new MemoryCacheWrapper("MemoryCache"))
          .BuildServiceProvider();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
            };

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
