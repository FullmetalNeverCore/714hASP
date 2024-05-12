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
        private readonly Model _model;
        private string _memoryplusnew;
        private readonly msgBuffer _mbuff;


        public UploadStuffController(Model model,msgBuffer mb,AKeyHandler api)
        {
            _model = model;
            _mbuff = mb;
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
                    Core.save_json(model.data, $"json_{_model.chr}/high_memory.json");
                }
                else
                {
                    try
                    {
                        Core.save_json(model.data, $"json_{_model.chr}/brain.json");
                        _mbuff.br = Core.open_json($"./json_{_model.chr}/brain.json");
                    }
                    catch(Exception ex)
                    {
                        Core.save_json($"upload_stuff: {ex.Message}", "./error.json");
                        Console.WriteLine(ex);
                    }

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

