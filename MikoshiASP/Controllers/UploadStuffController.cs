using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MikoshiASP.Controllers.Structures;
using MikoshiASP.Engine;
using Newtonsoft.Json.Linq;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MikoshiASP.Controllers
{
    [Route("upload_stuff")]
    [ApiController]
    public class UploadStuffController : Controller
    {
        private Core _core;
        private readonly Model _model;
        private string _memoryplusnew;
        private readonly msgBuffer _mbuff;


        public UploadStuffController(Model model,msgBuffer mb,AKeyHandler api)
        {
            _model = model;
            _mbuff = mb;
            _core = new Core(api.API_KEY);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Types model)
        {
            if (ModelState.IsValid)
            {
                //saving high memory and saving brain memory

                if(model.type == "high")
                {
                    _memoryplusnew = $"{model.data} {System.Environment.NewLine}";
                    _core.save_json(model.data, $"json_{_model.chr}/high_memory.json");
                }
                else
                {
                    _memoryplusnew = $"{_model.chr}:{model.data} {System.Environment.NewLine}";
                    Console.WriteLine("Saving brain...");
                    Console.WriteLine(_memoryplusnew);
                    _core.save_json(_memoryplusnew, $"json_{_model.chr}/brain.json");
                    _mbuff.br = _core.open_json($"./json_{_model.chr}/brain.json");
                }
                return Ok("Data updated successfully");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}

