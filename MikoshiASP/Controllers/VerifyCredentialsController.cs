using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MikoshiASP.Controllers.Structures;
using MikoshiASP.Engine;

namespace MikoshiASP.Controllers
{
    [Route("verify_credentials")]
    public class VerifyCredentialsController : Controller
    {
        private readonly Model _model;
        private readonly msgBuffer _mbuff;
        private readonly ILogger<VerifyCredentialsController> _logger;

        public VerifyCredentialsController(Model model, msgBuffer mb, ILogger<VerifyCredentialsController> logger)
        {
            _model = model;
            _mbuff = mb;
            _logger = logger;

            if (_model.temp == null) { _model.temp = "0.9"; }
            if (_model.fpen == null) { _model.fpen = "0.7"; }
            if (_model.ppen == null) { _model.ppen = "0.75"; }
        }

        [HttpPost]
        public IActionResult Post([FromBody] EngineUpdate model)
        {
            _logger.LogInformation("VERIFCRED: Updating chr");
            // Updating chr
            _model.chr = model.chara;

            // Fill the buffer
            _mbuff.text = null;
            _mbuff.br = Core.open_json($"./json_{_model.chr}/brain.json");
            _mbuff.hm = Core.open_json($"./json_{_model.chr}/high_memory.json");

            _logger.LogInformation("Updated buffer: br={br}, hm={hm}", _mbuff.br, _mbuff.hm);

            return Ok("Char updated");
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("VERIFCRED: Returning chr");
            // Return the character information
            var result = $"Chr {_model.chr} Temp: {_model.temp} Fpen {_model.fpen} Ppen {_model.ppen}";
            _logger.LogInformation(result);
            return Ok(result);
        }
    }
}
