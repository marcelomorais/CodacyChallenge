using CodacyChallenge.Application;
using CodacyChallenge.ConsoleApplication.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CodacyChallenge.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "";

            if (!args.Any())
            {
                Console.WriteLine("Please insert your GitHub url:");
                input = args.Any() ? args.First() : Console.ReadLine();
            }

            var serviceBuilder = new DependencyInjection().ServiceBuilder();

            var application = serviceBuilder.GetService<IStartApplication>();

            application.Execute(input);

            Console.ReadKey();
        }
    }
}
