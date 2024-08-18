using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FF.Api.Controllers
{
    public class LeaguesController : BaseController
    {
        private readonly Backend.Services.ILeagueService _leagueService;

        public LeaguesController(ILogger<LeaguesController> logger, FF.Backend.Services.ILeagueService leagueService) : base(logger)
        {
            _leagueService = leagueService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Result(_leagueService.GetAll());
        }

        [HttpGet]
        [Route("{leagueId}")]
        public IActionResult GetOne(int leagueId)
        {
            return Result(_leagueService.GetById(leagueId));

        }

    }
}
