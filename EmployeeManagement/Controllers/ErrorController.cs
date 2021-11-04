using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }
        [Route("Error/{StatusCode}")]
        public IActionResult HandlerError(int StatusCode)
        {
            switch (StatusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, The page cannot be found";
                    logger.LogWarning($"Error: {StatusCode}");
                    
                    break;
            }
            return View("NotFound");
        }
        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            logger.LogWarning($"Error: {exceptionHandlerFeature.Error}");
            logger.LogWarning($"ExceptionPath: {exceptionHandlerFeature.Path}");
            logger.LogWarning($"ErrorMessage: {exceptionHandlerFeature.Error.Message}");
            logger.LogWarning($"StackTrace: {exceptionHandlerFeature.Error.StackTrace}");

            return View();
        }
    }
}
