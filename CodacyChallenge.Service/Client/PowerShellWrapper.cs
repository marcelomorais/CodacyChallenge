using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace CodacyChallenge.Service.Client
{
    //Class built to isolate the GitCLIEngine so that can be completelly testable.
    public class PowershellWrapper : IPowershellWrapper
    {
        private static PowerShell _powerShell;
        public bool HadErrors { get; set; }
        public List<ErrorRecord> StreamErrors { get; set; }

        public PowershellWrapper()
        {
            _powerShell = PowerShell.Create();
        }

        public void AddScript(params string[] args)
        {
            foreach (var item in args)
            {
                _powerShell.AddScript(item);
            }
        }

        public List<PSObject> Invoke()
        {
            var psList = _powerShell.Invoke().ToList();
            this.HadErrors = _powerShell.HadErrors;
            this.StreamErrors = _powerShell?.Streams?.Error?.ToList();
            return psList;
        }

        public void Dispose()
        {
            _powerShell.Dispose();
        }
    }
}
