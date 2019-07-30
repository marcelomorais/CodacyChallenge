using System;
using System.Diagnostics;
using System.Management.Automation;
namespace CodacyChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please insert your GitHub url:");
            var input = Console.ReadLine();

            var clone = "git clone ";

            if (!input.Contains("github", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("The url must be from GitHub"); 
            }
            clone += input; 
            using (PowerShell powershell = PowerShell.Create())
            {
                powershell.AddScript(input);
                var result = powershell.Invoke();
            }
        }
    }
}
