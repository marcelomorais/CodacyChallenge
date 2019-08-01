using CodacyChallenge.Common.Models;
using CodacyChallenge.Common.Models.Exceptions;
using CodacyChallenge.Service.Client;
using CodacyChallenge.Service.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Management.Automation;
using System.Threading.Tasks;

namespace CodacyChallenge.Service.Tests
{
    [TestClass]
    public class GitCLIEngineTests
    {
        [TestMethod]
        public async Task GetCommitsWithPagination_ValidObject_ReturnPSObjects()
        {
            var request = new RequestObject { Url = string.Empty };
            var mockPowershell = new Mock<IPowershellWrapper>();
            mockPowershell.Setup(x => x.Invoke()).Returns(new List<PSObject> { new PSObject() });
            var gitCLIEngine = new GitCLIEngine(mockPowershell.Object);

            var response = await gitCLIEngine.GetCommitsWithPagination(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(CLIException))]
        public async Task GetCommitsWithPagination_InvokeDoNotReturnNothingAndHadErrors_ThrowCLIException()
        {
            var request = new RequestObject { Url = string.Empty };
            var mockPowershell = new Mock<IPowershellWrapper>();
            mockPowershell.Setup(x => x.Invoke()).Returns(new List<PSObject>());
            mockPowershell.SetupProperty(x => x.HadErrors, true);
            mockPowershell.SetupProperty(x => x.StreamErrors, new List<ErrorRecord>());
            var gitCLIEngine = new GitCLIEngine(mockPowershell.Object);

            var response = await gitCLIEngine.GetCommitsWithPagination(request);
        }
    }
}
