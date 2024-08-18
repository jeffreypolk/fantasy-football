using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FF.Api.Controllers
{
    public class TeamsController : BaseController
    {
        private readonly Backend.Services.ITeamService _teamService;

        public TeamsController(ILogger<TeamsController> logger, FF.Backend.Services.ITeamService teamService) : base(logger)
        {
            _teamService = teamService;
        }

        [HttpGet]
        [Route("{teamId}")]
        public IActionResult GetOne(int teamId)
        {
            return Result(_teamService.GetById(teamId));

        }
        [HttpGet]
        public IActionResult GetByLeagueAndYear(int leagueId, int year)
        {
            return Result(_teamService.GetByLeagueYear(leagueId, year));
        }
    }
}
