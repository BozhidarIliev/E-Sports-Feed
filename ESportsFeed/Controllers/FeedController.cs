using Azure.Core;
using ESportsFeed.Data;
using ESportsFeed.Web.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System;
using ESportsFeed.Services.Data.Interfaces;

namespace ESportsFeed.Web.Controllers
{
    namespace ESportsFeed.Controllers
    {
        [Route("api/matches")]
        [ApiController]
        public class MatchesController : ControllerBase
        {
            private readonly ApplicationDbContext _context; // Replace "YourDataContext" with your actual data context class name
            private readonly IMatchService matchService;

            public MatchesController(ApplicationDbContext context, IMatchService matchService)
            {
                _context = context;
                this.matchService = matchService;
            }

            [HttpGet("matches/next24hours")]
            public ActionResult<List<MatchDTO>> GetMatchesStartingInNext24Hours()
            {
                try
                {
                    var matches = matchService.GetMatchesStartingIn24Hours();

                    return matches;
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                    throw;
                }
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<MatchDetailsDTO>> GetMatchById(string id)
            {
                try
                {
                    var match = await matchService.GetMatchById(id);
                    if (match == null)
                    {
                        return NotFound();
                    }

                    return match;
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                    throw;
                }
            }

        }
    }
}
