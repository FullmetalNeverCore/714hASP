using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MikoshiASP.Controllers.Structures;
using MikoshiASP.Engine;

namespace MikoshiASP.Controllers
{
    [Route("api/setakey")]
    public class SetAKeyController : Controller
    {
        private readonly AKeyHandler _API;
        private readonly ILogger<SetAKeyController> _logger;

        public SetAKeyController(AKeyHandler api, ILogger<SetAKeyController> logger)
        {
            // Get API KEY singleton instance
            _API = api;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok($"{_API.API_KEY}");
        }

        [HttpPost]
        public IActionResult Post([FromBody] Types value)
        {
            try
            {
                _logger.LogInformation("Updating API_KEY....");
                _API.API_KEY = value.data;
                _logger.LogInformation("API_KEY updated successfully.");
                return Ok("Ok!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating API_KEY.");
                Core.save_json($"api/setakey: {ex.Message}", "./error.json");
                return BadRequest(ex.Message);
            }
        }
    }
}
