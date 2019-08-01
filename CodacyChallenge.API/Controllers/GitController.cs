using CodacyChallenge.Common.Enumerators;
using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Common.Models;
using CodacyChallenge.Common.Models.Exceptions;
using CodacyChallenge.Utils;
using CodacyChallenge.Utils.Validators;
using Microsoft.AspNetCore.Mvc;
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

                var commitList = await apiEngine.GetAllCommits(requestObject.Url).ConfigureAwait(false);

                return Ok(commitList.Paginate(requestObject));
            }
            catch (HttpRequestException)
            {
                var cliEngine = _gitEngine(RequestType.CLI);

                var commitList = await cliEngine.GetAllCommits(requestObject.Url).ConfigureAwait(false);

                return Ok(commitList.Paginate(requestObject));
            }
            catch (CLIException ex)
            {
                return StatusCode(500, ex);
            }
            catch (KeyNotFoundException)
            {
                return BadRequest();
            }
            catch
            {
                return StatusCode(500);
            }

        }

    }
}
