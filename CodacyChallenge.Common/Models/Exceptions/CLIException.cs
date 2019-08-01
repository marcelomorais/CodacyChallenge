using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace CodacyChallenge.Common.Models.Exceptions
{
    public class CLIException : Exception
    {
        public string ErrorMessage { get; set; }
        public List<ErrorRecord> StreamErrors = new List<ErrorRecord>();

        public CLIException(string message) : base(message)
        {

        }

        public CLIException(PSDataStreams stream)
        {
            StreamErrors.AddRange(stream.Error.ReadAll());
        }
    }
}
