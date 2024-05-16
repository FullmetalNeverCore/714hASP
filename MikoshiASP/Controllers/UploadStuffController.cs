using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MikoshiASP.Controllers.Structures;
using MikoshiASP.Engine;
using Newtonsoft.Json.Linq;

namespace MikoshiASP.Controllers
{
    [Route("upload_stuff")]
    [ApiController]
    public class UploadStuffController : Controller
    {
        private readonly Model _model;
        private string _memoryplusnew;
        private readonly msgBuffer _mbuff;
        private readonly ILogger<UploadStuffController> _logger;

        public UploadStuffController(Model model, msgBuffer mb, AKeyHandler api, ILogger<UploadStuffController> logger)
        {
            _model = model;
            _mbuff = mb;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Types model)
        {
            if (ModelState.IsValid)
            {
                // Saving high memory and brain memory
                if (model.type == "high")
                {
                    _memoryplusnew = $"{model.data} {Environment.NewLine}";
                    Core.save_json(model.data, $"json_{_model.chr}/high_memory.json");
                    _logger.LogInformation("High memory updated successfully for character {Character}.", _model.chr);
                }
                else
                {
                    try
                    {
                        Core.save_json(model.data, $"json_{_model.chr}/brain.json");
                        _mbuff.br = Core.open_json($"./json_{_model.chr}/brain.json");
                        _logger.LogInformation("Brain memory updated successfully for character {Character}.", _model.chr);
                    }
                    catch (Exception ex)
                    {
                        Core.save_json($"upload_stuff: {ex.Message}", "./error.json");
                        _logger.LogError(ex, "An error occurred while updating brain memory for character {Character}.", _model.chr);
                    }
                }
                return Ok("Data updated successfully");
            }
            else
            {
                _logger.LogWarning("Invalid model state received.");
                return BadRequest(ModelState);
            }
        }
    }
}
