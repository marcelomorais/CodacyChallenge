using CodacyChallenge.API.Client;
using CodacyChallenge.Common.Enumerators;
using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Common.Models.Configuration;
using CodacyChallenge.Service.Client;
using CodacyChallenge.Service.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace CodacyChallenge.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddOptions()
            .Configure<GitHubEndpoints>(options => Configuration.GetSection("GitHubApi").Bind(options))
            .AddTransient<GitCLIEngine>()
            .AddTransient<GitAPIEngine>()
            .AddSingleton<IPowershellWrapper, PowershellWrapper>()
            .AddSingleton<HttpClient>(new HttpClient())
            .AddSingleton<IApiClient, ApiClient>()
            .AddSingleton<IHttpClientWrapper, HttpClientWrapper>()
            .AddTransient<Func<RequestType, IGitEngine>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case RequestType.CLI:
                        return serviceProvider.GetService<GitCLIEngine>();
                    case RequestType.API:
                        return serviceProvider.GetService<GitAPIEngine>();
                    default:
                        throw new KeyNotFoundException();
                }
            })
              .BuildServiceProvider();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
