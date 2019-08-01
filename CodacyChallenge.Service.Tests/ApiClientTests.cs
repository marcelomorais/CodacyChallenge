using CodacyChallenge.API.Client;
using CodacyChallenge.Service.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodacyChallenge.Service.Tests
{
    [TestClass]
    public class ApiClientTests
    {
        [TestMethod]
        public async Task GetAsync_ReturnAcceptedStatusCode_Pass()
        {
            var httpClient = new Mock<IHttpClientWrapper>();
            httpClient.Setup(x => x.GetAsync(string.Empty)).ReturnsAsync(new HttpResponseMessage { Content = new StringContent("{}"), StatusCode = System.Net.HttpStatusCode.Accepted });
            httpClient.Setup(x => x.AddHeaders(It.IsAny<Dictionary<string, string>>()));
            var client = new ApiClient(httpClient.Object);

            var response = await client.GetAsync<object>(string.Empty);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task GetAsync_StatusCodeDifferentOfAccepted_ThrowException()
        {
            var httpClient = new Mock<IHttpClientWrapper>();
            httpClient.Setup(x => x.GetAsync(string.Empty)).ReturnsAsync(new HttpResponseMessage { Content = new StringContent("{}"), StatusCode = System.Net.HttpStatusCode.InternalServerError });
            httpClient.Setup(x => x.AddHeaders(It.IsAny<Dictionary<string, string>>()));
            var client = new ApiClient(httpClient.Object);

            var response = await client.GetAsync<object>(string.Empty);
        }
    }
}
