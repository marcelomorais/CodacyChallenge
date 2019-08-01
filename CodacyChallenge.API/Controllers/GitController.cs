using CodacyChallenge.Common.Enumerators;
using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Common.Models;
using CodacyChallenge.Utils.Validators;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions;
using CodacyChallenge.Utils;
using System.Net.Http;

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
        public async Task<ActionResult<IEnumerable<string>>> AllCommits([FromQuery]RequestObject requestObject)
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
            catch (HttpRequestException ex)
            {
                var cliEngine = _gitEngine(RequestType.Shell);

                var commitList = await cliEngine.GetAllCommits(requestObject.Url).ConfigureAwait(false);

                return Ok(commitList.Paginate(requestObject));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }

    }
}
