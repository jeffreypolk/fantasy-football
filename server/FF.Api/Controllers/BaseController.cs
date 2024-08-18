using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FF.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        private readonly ILogger<BaseController> _logger;

        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        protected JsonResult Result()
        {
            var res = new JsonResult(FF.Backend.Results.Result.Success());
            res.StatusCode = 200;
            return res;
        }

        protected JsonResult Result(FF.Backend.Results.IResult result)
        {
            var res = new JsonResult(result);
            res.StatusCode = result.StatusCode;
            return res;
        }
    }
}
