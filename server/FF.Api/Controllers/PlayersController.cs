using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FF.Api.Controllers
{
    public class PlayersController : BaseController
    {
        private readonly Backend.Services.IPlayerService _playerService;

        public PlayersController(ILogger<PlayersController> logger, FF.Backend.Services.IPlayerService playerService) : base(logger)
        {
            _playerService = playerService;
        }

        [HttpGet]
        [Route("{playerId}")]
        public IActionResult GetOne(int playerId)
        {
            return Result(_playerService.GetById(playerId));

        }
        [HttpGet]
        public IActionResult GetByYear(int year)
        {
            return Result(_playerService.GetByYear(year));
        }
    }
}
