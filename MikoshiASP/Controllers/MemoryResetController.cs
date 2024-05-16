using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MikoshiASP.Controllers.Structures;
using MikoshiASP.Engine;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MikoshiASP.Controllers
{
    [Route("api/memoryreset")]
    public class MemoryResetController : Controller
    {
        private readonly Model _model;
        private readonly msgBuffer _mbuff;  

        public MemoryResetController(Model model,msgBuffer mb)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _mbuff = mb ?? throw new ArgumentNullException(nameof(mb));
        }

        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            try
            {
                string[] fullmemory = Core.open_json($"./json_{name}/brain.json").Split("N:");
                Console.WriteLine(fullmemory[0]); //inital memory
                Core.save_json(fullmemory[0], $"./json_{name}/brain.json"); //saving updated memory
                _mbuff.br = Core.open_json($"./json_{name}/brain.json");
           
            }
            catch (IndexOutOfRangeException ex)
            {
                return BadRequest($"IndexOutOfRange - {ex.Message}");
            }
            catch (NullReferenceException ex)
            {
                return BadRequest($"NullReference - {ex.Message}");
            }
            return Ok("Operation complete! 200");
        }
    }
}

