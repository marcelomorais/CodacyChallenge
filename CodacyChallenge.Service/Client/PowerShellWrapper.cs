using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace CodacyChallenge.Service.Client
{
    //Class built to isolate the GitCLIEngine so that can be completelly testable.
    public class PowershellWrapper : IPowershellWrapper
    {
        private bool _hadErrors = _powerShell.HadErrors;
        private List<ErrorRecord> _streamErrors = _powerShell.Streams?.Error?.ToList();

        private static PowerShell _powerShell;
        public bool HadErrors { get => _hadErrors; set => this._hadErrors = value; }
        public List<ErrorRecord> StreamErrors { get => _streamErrors; set => this._streamErrors = value; }

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
            return _powerShell.Invoke().ToList();
        }

        public void Dispose()
        {
            _powerShell.Dispose();
        }
    }
}
