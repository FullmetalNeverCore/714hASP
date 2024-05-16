using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MikoshiASP.Engine;

namespace MikoshiASP.Controllers.Structures
{
    [Route("nn_vals")]
    [ApiController]
    public class NNValsController : Controller
    {
        private readonly Model _model;
        private readonly ILogger<NNValsController> _logger;

        public NNValsController(Model model, ILogger<NNValsController> logger)
        {
            _model = model;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody] NNValues nnv)
        {
            try
            {
                var possibleValues = new Dictionary<string, Action<string>>
                {
                    { "rnd", val => _model.temp = val },
                    { "fpen", val => _model.fpen = val },
                    { "ppen", val => _model.ppen = val }
                };

                _logger.LogInformation("Changing Model...");
                possibleValues[nnv.type](nnv.val);

                _logger.LogInformation("Model changed: type={type}, value={value}", nnv.type, nnv.val);
                return Ok();
            }
            catch (Exception ex)
            {
                Core.save_json($"nn_vals: Bad request, probably wrong body. {ex.Message}", "./error.json");
                _logger.LogError(ex, "An error occurred while changing model values.");
                return BadRequest($"Bad request, probably wrong body. {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            var modelInfo = $"CURRENT MODEL = chr {_model.chr} temp {_model.temp} fpen {_model.fpen} ppen {_model.ppen}";
            _logger.LogInformation(modelInfo);
            return Ok(modelInfo);
        }
    }
}
