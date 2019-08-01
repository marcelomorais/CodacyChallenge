using AutoFixture;
using AutoFixture.AutoMoq;
using CodacyChallenge.API.Controllers;
using CodacyChallenge.Common.Enumerators;
using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Common.Models;
using CodacyChallenge.Common.Models.Configuration;
using CodacyChallenge.Common.Models.Exceptions;
using CodacyChallenge.Service.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodacyChallenge.API.Tests
{
    [TestClass]
    public class GitControllerTests
    {
        [TestMethod]
        public async Task GetAllCommits_InvalidUrl_BadRequest()
        {
            var _mockGitEngine = new Mock<Func<RequestType, IGitEngine>>();
            var controller = new GitController(_mockGitEngine.Object);
            var request = new RequestObject { Url = "test", RequestType = RequestType.API };

            var result = await controller.GetAllCommits(request).ConfigureAwait(false);
            var httpResponse = result.Result as BadRequestResult;

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task GetAllCommits_ValidUrl_OkResult()
        {
            var request = new RequestObject { Url = "https://github.com/marcelomorais/CodacyChallenge", RequestType = RequestType.API };

            var fixture = new Fixture()
                 .Customize(new AutoMoqCustomization());

            var mock2 = fixture.Freeze<Mock<IGitEngine>>();
            mock2.Setup(x => x.GetCommitsWithPagination(request))
            .ReturnsAsync(new List<GitResponse> { new GitResponse() });

            var mock = fixture.Freeze<Mock<Func<RequestType, IGitEngine>>>();
            mock.Setup(x => x.Invoke(RequestType.API)).Returns(mock2.Object);

            var controller = new GitController(mock.Object);

            var result = await controller.GetAllCommits(request).ConfigureAwait(false);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }


        [TestMethod]
        public async Task GetAllCommits_ThrowHttpRequestException_OkResultCallinGitCLIEngine()
        {
            var request = new RequestObject { Url = "https://github.com/marcelomorais/CodacyChallenge", RequestType = RequestType.API };

            var fixture = new Fixture()
                 .Customize(new AutoMoqCustomization());

            var mock2 = fixture.Create<Mock<IGitEngine>>();
            mock2.Setup(x => x.GetCommitsWithPagination(request))
                .ThrowsAsync(new HttpRequestException());

            var mock3 = fixture.Create<Mock<IGitEngine>>();
            mock3.Setup(x => x.GetCommitsWithPagination(request))
                .ReturnsAsync(new List<GitResponse> { new GitResponse() });


            var mock = fixture.Freeze<Mock<Func<RequestType, IGitEngine>>>();
            mock.Setup(x => x.Invoke(RequestType.API)).Returns(mock2.Object);
            mock.Setup(x => x.Invoke(RequestType.CLI)).Returns(mock3.Object);

            var controller = new GitController(mock.Object);

            var result = await controller.GetAllCommits(request).ConfigureAwait(false);

            mock2.Verify(x => x.GetCommitsWithPagination(request), Times.Once);
            mock3.Verify(x => x.GetCommitsWithPagination(request), Times.Once);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetAllCommits_ThrowHCLIException_Return500StatusCode()
        {
            var request = new RequestObject { Url = "https://github.com/marcelomorais/CodacyChallenge", RequestType = RequestType.API };

            var fixture = new Fixture()
                 .Customize(new AutoMoqCustomization());

            var mock2 = fixture.Create<Mock<IGitEngine>>();
            mock2.Setup(x => x.GetCommitsWithPagination(request))
                .ThrowsAsync(new CLIException(string.Empty));


            var mock = fixture.Freeze<Mock<Func<RequestType, IGitEngine>>>();
            mock.Setup(x => x.Invoke(RequestType.API)).Returns(mock2.Object);

            var controller = new GitController(mock.Object);

            var result = await controller.GetAllCommits(request).ConfigureAwait(false);
            var status = result.Result as ObjectResult;

            mock2.Verify(x => x.GetCommitsWithPagination(request), Times.Once);
            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            Assert.AreEqual(status.StatusCode, 500);
        }


        [TestMethod]
        public async Task GetAllCommits_ThrowHKeyNotFoundException_BadRequest()
        {
            var request = new RequestObject { Url = "https://github.com/marcelomorais/CodacyChallenge", RequestType = RequestType.Unknown };

            var fixture = new Fixture()
                 .Customize(new AutoMoqCustomization());


            var mock = fixture.Freeze<Mock<Func<RequestType, IGitEngine>>>();
            mock.Setup(x => x.Invoke(RequestType.Unknown))
                .Throws(new KeyNotFoundException());

            var controller = new GitController(mock.Object);

            var result = await controller.GetAllCommits(request).ConfigureAwait(false);
            var status = result.Result as BadRequestObjectResult;
            
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual(status.StatusCode, 400);
        }
    }
}
