using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FF.Api.Controllers
{
    public class ManagersController : BaseController
    {
        private readonly Backend.Services.IManagerService _managerService;

        public ManagersController(ILogger<ManagersController> logger, FF.Backend.Services.IManagerService managerService) : base(logger)
        {
            _managerService = managerService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Result(_managerService.GetAll());
        }

        [HttpGet]
        [Route("{managerId}")]
        public IActionResult GetOne(int managerId)
        {
            return Result(_managerService.GetById(managerId));

        }
    }
}
