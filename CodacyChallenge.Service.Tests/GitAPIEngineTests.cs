using AutoFixture;
using AutoFixture.AutoMoq;
using CodacyChallenge.API.Client;
using CodacyChallenge.Common.Models;
using CodacyChallenge.Common.Models.Configuration;
using CodacyChallenge.Service.Implementations;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodacyChallenge.Service.Tests
{
    [TestClass]
    public class GitAPIEngineTests
    {
        [TestMethod]
        public async Task GetCommitsWithPagination_ObjectWithEmptyUrl_DoNotThrowException()
        {
            var request = new RequestObject { Url = string.Empty };
            var apiClient = new Mock<IApiClient>();

           var options = Options.Create(new GitHubEndpoints { GetAllCommits = string.Empty });

            apiClient.Setup(x => x.GetAsync<List<GitResponse>>(It.IsAny<string>())).ReturnsAsync(new List<GitResponse> { new GitResponse() });
            var gitApiEngine = new GitAPIEngine(options, apiClient.Object);
             
            var response = await gitApiEngine.GetCommitsWithPagination(request);

            Assert.IsNotNull(response);
        }
    }
}
