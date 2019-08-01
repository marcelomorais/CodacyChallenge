using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Common.Models;
using CodacyChallenge.ConsoleApplication.Configuration;
using CodacyChallenge.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodacyChallenge.Application
{
    public class StartApplication : IStartApplication
    {
        private IGitEngine _gitEngine;
        private Configuration _configuration;
        private int page = 1;

        public StartApplication(IOptions<Configuration> configuration, IGitEngine gitEngine)
        {
            _configuration = configuration.Value;
            _gitEngine = gitEngine;
        }
        public async Task Execute(string url)
        {
            Console.WriteLine("Pulling commits...\n");

            var commits = await _gitEngine.GetAllCommits(url);

            var commitList = commits.BreakInPages(_configuration.ItemsPerPage);

            if (commitList.Any())
            {
                CallNextPage(commitList);
            }
        }

        private void CallNextPage(List<List<GitResponse>> commitList)
        {
            if (page <= commitList.Count)
            {
                foreach (var item in commitList.Skip(page - 1).Take(1))
                {
                    item.ForEach(x =>
                    {
                        Console.WriteLine(JsonConvert.SerializeObject(x));
                    });
                }

                Console.WriteLine($"\nThis Repository has a total of {commitList.Count} pages with {_configuration.ItemsPerPage} elements per page.");
                Console.WriteLine($"Enter the page number that you want to see or press N to go to the next page.");
                Console.WriteLine($"You are in the page: {page}.\n");

                CheckPressedKey();
                CallNextPage(commitList);
            }
            else
            {
                Console.WriteLine($"\nThe page {page} does not exist.\n");
                Console.WriteLine($"Please try a different page or press ESC to exit.\n");
                CheckPressedKey();
                CallNextPage(commitList);
            }

        }

        private void CheckPressedKey()
        {
            int result = page;
            var keyPressed = new ConsoleKeyInfo();

            while (!(keyPressed.Key == ConsoleKey.N || int.TryParse(keyPressed.KeyChar.ToString(), out result)))
            {
                keyPressed = Console.ReadKey(true);

                if (keyPressed.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
            }

            page = keyPressed.Key == ConsoleKey.N ? page++ : result;
        }
    }
}
