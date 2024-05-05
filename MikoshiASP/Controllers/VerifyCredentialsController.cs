using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MikoshiASP.Controllers.Structures;
using MikoshiASP.Engine;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MikoshiASP.Controllers
{
    [Route("verify_credentials")]
    public class VerifyCredentialsController : Controller
    {
        private readonly Model _model;
        private readonly msgBuffer _mbuff;
        private Core _core = new Core();

        public VerifyCredentialsController(Model model,msgBuffer mb)
        {
            _model = model;
            _mbuff = mb;

            if(_model.temp == null) { _model.temp = "0.9"; }
            if(_model.fpen == null) { _model.fpen = "0.7"; }
            if(_model.ppen == null) { _model.ppen = "0.75"; }
        }
        [HttpPost]
        public IActionResult Post([FromBody] EngineUpdate model)
        {
            Console.WriteLine("VERIFCRED: Updating chr");
            //updating chr

            _model.chr = model.chara;

            //string json = JsonSerializer.Serialize(model, new JsonSerializerOptions
            //{
            //    WriteIndented = true
            //});

            //Console.WriteLine("JSON Structure:");
            //Console.WriteLine(json);

            //fill the buffer
            _mbuff.text = null;
            _mbuff.br = _core.open_json($"./json_{_model.chr}/brain.json");
            _mbuff.hm = _core.open_json($"./json_{_model.chr}/high_memory.json");

            Console.WriteLine(_mbuff.br);
            Console.WriteLine(_mbuff.hm);

            return Ok("Char updated");
        }

        [HttpGet]
        public IActionResult Get()
        {
            Console.WriteLine("VERIFCRED: Returning chr");
            //no chr return null
            //204? 
            return Ok($"Chr {_model.chr} Temp: {_model.temp} Fpen {_model.fpen} Ppen {_model.ppen}") != null ? Ok(_model.chr) : Ok(null);
        }
    }
}

