using CodacyChallenge.API.Mappers;
using CodacyChallenge.Common.Enumerators;
using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Common.Models;
using CodacyChallenge.Common.Models.Exceptions;
using CodacyChallenge.Utils;
using CodacyChallenge.Utils.Validators;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodacyChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitController : ControllerBase
    {
        private Func<RequestType, IGitEngine> _gitEngine;

        public GitController(Func<RequestType, IGitEngine> _gitEngineDelegate)
        {
            _gitEngine = _gitEngineDelegate;
        }

        // GET api/Git/Repo/Commits
        [HttpGet("Repo/Commits")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllCommits([FromQuery]RequestObject requestObject)
        {
            if (!Validator.ValidateUrl(requestObject.RequestType, requestObject.Url))
            {
                return BadRequest();
            }

            try
            {
                var response = await RunService(requestObject.RequestType, requestObject);

                return Ok(response);
            }
            //If GitHub API return a TimeoutException or HttpRequestException we will go to git CLI to get the commit list
            catch (Exception ex) when (ex is TimeoutException || ex is HttpRequestException)
            {
                try
                {
                    var response = await RunService(RequestType.CLI, requestObject);

                    return Ok(response);
                }
                catch (CLIException x)
                {
                    return StatusCode(500, x.StreamErrors);
                }
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        private async Task<string> RunService(RequestType requestType, RequestObject requestObject)
        {
            var gitEngine = _gitEngine(requestType);

            var commitList = await gitEngine.GetCommitsWithPagination(requestObject).ConfigureAwait(false);

            var responseObject = commitList.ToResponseObject(requestObject);

            //This line is only to filter and remove the null fields from the Json object.
            var filteredResponse = JsonConvert.SerializeObject(responseObject);

            return filteredResponse;
        }

    }
}
