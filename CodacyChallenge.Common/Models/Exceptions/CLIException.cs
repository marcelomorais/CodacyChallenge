using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace CodacyChallenge.Common.Models.Exceptions
{
    public class CLIException : Exception
    {
        public List<ErrorRecord> StreamErrors = new List<ErrorRecord>();

        public CLIException()
        {

        }

        public CLIException(string message) : base(message)
        {

        }

        public CLIException(List<ErrorRecord> stream)
        {
            StreamErrors.AddRange(stream);
        }
    }
}
