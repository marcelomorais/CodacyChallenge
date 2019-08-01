using CodacyChallenge.Common.Enumerators;
using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Common.Models.Configuration;
using CodacyChallenge.Service.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

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
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

          services.AddOptions()
          .Configure<GitHubEndpoints>(options => Configuration.GetSection("GitHubApi").Bind(options))
          .AddTransient<GitCLIEngine>()
          .AddTransient<GitAPIEngine>()
          .AddTransient<Func<RequestType, IGitEngine>>(serviceProvider => key =>
          {
              switch (key)
              {
                  case RequestType.Shell:
                      return serviceProvider.GetService<GitCLIEngine>();
                  case RequestType.API:
                      return serviceProvider.GetService<GitAPIEngine>();
                  default:
                      throw new KeyNotFoundException();
              }
          })
            .BuildServiceProvider();
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
