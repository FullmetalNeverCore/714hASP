using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MikoshiASP.Controllers.Structures
{
    [Route("nn_vals")]
    [ApiController]
    public class NNValsController : Controller
    {
        private readonly Model _model;

        public NNValsController(Model model)
        {
            _model = model;
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
                Console.WriteLine("Chaning Model...");
                possibleValues[nnv.type](nnv.val);
                return Ok();
            }
            catch(Exception ex)
            {
          
                return BadRequest($"Bad request,probably wrong body.{ex}");
            }
         
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok($"CURRENT MODEL = chr {_model.chr} temp {_model.temp} fpen {_model.fpen} ppen {_model.ppen}");
        }
    }
}
