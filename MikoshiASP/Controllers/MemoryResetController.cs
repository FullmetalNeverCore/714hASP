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
    [Route("api/memoryreset")]
    public class MemoryResetController : Controller
    {
        private readonly Model _model;
        private readonly msgBuffer _mbuff;
        private readonly ILogger<MemoryResetController> _logger;

        public MemoryResetController(Model model, msgBuffer mb, ILogger<MemoryResetController> logger)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _mbuff = mb ?? throw new ArgumentNullException(nameof(mb));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            try
            {
                string[] fullmemory = Core.open_json($"./json_{name}/brain.json").Split("N:");
                _logger.LogInformation("Initial memory: {initialMemory}", fullmemory[0]);

                Core.save_json(fullmemory[0], $"./json_{name}/brain.json"); // Saving updated memory
                _mbuff.br = Core.open_json($"./json_{name}/brain.json");

                _logger.LogInformation("Memory reset complete for {name}", name);
            }
            catch (IndexOutOfRangeException ex)
            {
                _logger.LogError(ex, "IndexOutOfRangeException occurred while resetting memory for {name}", name);
                return BadRequest($"IndexOutOfRange - {ex.Message}");
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "NullReferenceException occurred while resetting memory for {name}", name);
                return BadRequest($"NullReference - {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while resetting memory for {name}", name);
                return BadRequest($"Error - {ex.Message}");
            }

            return Ok("Operation complete! 200");
        }
    }
}
