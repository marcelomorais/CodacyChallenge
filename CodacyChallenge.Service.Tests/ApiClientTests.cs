using CodacyChallenge.API.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodacyChallenge.Service.Tests
{
    [TestClass]
    public class ApiClientTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var client = new ApiClient();

            client.GetAsync(string.Empty);
        }
    }
}
