using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace CodacyChallenge.Service.Client
{
    public interface IPowershellWrapper : IDisposable
    {
        bool HadErrors { get; set; }
        List<ErrorRecord> StreamErrors { get; set; }
        void AddScript(params string[] args);
        List<PSObject> Invoke();
    }
}