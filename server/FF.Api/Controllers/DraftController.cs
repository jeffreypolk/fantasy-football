using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FF.Backend.Services;
using dto = FF.Backend.Services.DTO;

namespace FF.Api.Controllers
{
    public class DraftController : BaseController
    {
        private readonly IDraftService _draftService;
        private readonly FF.Backend.Repositories.Framework.IUnitOfWork _unitOfWork;

        public DraftController(ILogger<TeamsController> logger, IDraftService draftService, FF.Backend.Repositories.Framework.IUnitOfWork unitOfWork) : base(logger)
        {
            _draftService = draftService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("board")]
        public IActionResult GetBoard(int leagueId, int year)
        {
            return Result(_draftService.GetBoard(leagueId, year));
        }

        [HttpGet]
        [Route("teams")]
        public IActionResult GetTeams(int leagueId, int year)
        {
            return Result(_draftService.GetTeams(leagueId, year));
        }

        [HttpGet]
        [Route("positions")]
        public IActionResult GetPositions(int leagueId, int year)
        {
            return Result(_draftService.GetPickPositions(leagueId, year));

        }

        [HttpGet]
        [Route("players")]
        public IActionResult GetPlayers(int leagueId, int year)
        {
            return Result(_draftService.GetPlayers(leagueId, year));
        }

        [HttpPost]
        [Route("pick")]
        public IActionResult MakePick(dto.Draft.Pick pick)
        {
            var ret = Result(_draftService.MakePick(pick));
            _unitOfWork.CommitChanges();
            return ret;
        }
    }
}
