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
                var apiEngine = _gitEngine(requestObject.RequestType);

                var commitList = await apiEngine.GetCommitsWithPagination(requestObject).ConfigureAwait(false);

                var responseObject = commitList.Paginate(requestObject).ToResponseObject(requestObject);

                //This line is only to filter and remove the null fields from the Json object.
                var filteredResponse = JsonConvert.SerializeObject(responseObject);

                return Ok(filteredResponse);
            }
            //If GitHub API return any error or exception we will go to git CLI to get the commit list
            catch (HttpRequestException)
            {
                var cliEngine = _gitEngine(RequestType.CLI);

                var commitList = await cliEngine.GetCommitsWithPagination(requestObject).ConfigureAwait(false);

                var responseObject = commitList.Paginate(requestObject).ToResponseObject(requestObject);

                //This line is only to filter and remove the null fields from the Json object.
                var filteredResponse = JsonConvert.SerializeObject(responseObject);

                return Ok(filteredResponse);
            }
            catch (CLIException ex)
            {
                return StatusCode(500, ex);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

    }
}
