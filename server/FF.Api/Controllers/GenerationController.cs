using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FF.Api.Controllers
{
    public class GenerationController : BaseController
    {
        private readonly Backend.Services.IGenerationService _generationService;

        public GenerationController(ILogger<GenerationController> logger, FF.Backend.Services.IGenerationService generationService) : base(logger)
        {
            _generationService = generationService;
        }

        [HttpPost]
        [Route("DataFiles")]
        public IActionResult GenerateDataFiles()
        {
            return Result(_generationService.GenerateDataFiles());
        }

    }
}
