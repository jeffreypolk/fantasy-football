using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FF.Backend.Services;
using dto = FF.Backend.Services.DTO;
using System;

namespace FF.Api.Controllers
{
    public class StatsController : BaseController
    {
        private readonly IStatsService _statsService;
        
        public StatsController(ILogger<TeamsController> logger, IStatsService statsService) : base(logger)
        {
            _statsService = statsService;
        }

        [HttpGet]
        public IActionResult GetStats(int leagueId, string year, string managerType)
        {
            var yearNum = 0;
            var yearStart = 0;
            var yearEnd = 0;
            if (int.TryParse(year, out yearNum))
            {
                yearStart = yearNum;
                yearEnd = yearNum;
            }
            else
            {
                switch(year.ToLower())
                {
                    case "all":
                        yearStart = 2003;
                        yearEnd = DateTime.Now.Year;
                        break;
                    case "last5":
                        yearStart = DateTime.Now.AddYears(-5).Year;
                        yearEnd = DateTime.Now.Year;
                        break;
                    case "last10":
                        yearStart = DateTime.Now.AddYears(-10).Year;
                        yearEnd = DateTime.Now.Year;
                        break;
                }
            }

            var includeActive = true;
            if (managerType.ToLower() == "all")
            {
                includeActive = false;
            }
            return Result(_statsService.GetStats(leagueId, yearStart, yearEnd, includeActive));
        }
    }
}
