using CodacyChallenge.Application;
using CodacyChallenge.Common.Enumerators;
using CodacyChallenge.ConsoleApplication.Configuration;
using CodacyChallenge.Utils.Validators;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CodacyChallenge.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = string.Empty;

            if (!args.Any())
            {
                Console.WriteLine("Please insert your GitHub url:");
                input = Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"We are working with this url: '{args[0]}'.");
                input = args[0];
            }

            Console.WriteLine("Validating your URL...");

            if (!Validator.ValidateUrl(RequestType.Shell ,input))
            {
                Console.WriteLine("Your Repository URL does not match with the requirements.");
                Console.WriteLine("Please be sure that your URL is from valid a GitHub Repository.");
            }

            Console.WriteLine("URL OK.");

            var serviceBuilder = new DependencyInjection().ServiceBuilder();

            var application = serviceBuilder.GetService<IStartApplication>();

            application.Execute(input);
        }
    }
}
