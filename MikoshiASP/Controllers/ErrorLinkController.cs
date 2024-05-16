using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MikoshiASP.Engine;

namespace MikoshiASP.Controllers
{
    [Route("api/errormsg")]
    [ApiController]
    public class ErrorLinkController : ControllerBase
    {
        private readonly ILogger<ErrorLinkController> _logger;

        public ErrorLinkController(ILogger<ErrorLinkController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            // TODO: Add RAM consumption to error message information, and other host info.
            try
            {
                // Obtain the last error message from source 
                string errorMessage = Core.open_json("./error.json");
                _logger.LogInformation("Last Exception: {errorMessage}", errorMessage);

                Dictionary<string, string> response = new Dictionary<string, string>
                {
                    { "err", errorMessage }
                };
                _logger.LogInformation("Response: {response}", response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during processing
                Core.save_json($"api/errormsg : {ex.Message}", "./error.json");
                _logger.LogError(ex, "An error occurred while processing the request.");
                return BadRequest(ex.Message);
            }
        }
    }
}
