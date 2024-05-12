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
    [Route("api/setakey")]
    public class SetAKeyController : Controller
    {
        private AKeyHandler _API;

        public SetAKeyController(AKeyHandler api)
        {
            //get API KEY signletone instance
            _API = api; 
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
                Console.WriteLine("Updating API_KEY....");
                _API.API_KEY = value.data;
                return Ok("Ok!");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
                Core.save_json($"api/setakey: {ex.Message}", "./error.json");
                return BadRequest(ex.Message);
            }
        }

    }
}

