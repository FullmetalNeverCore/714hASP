using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MikoshiASP.Controllers.Structures;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MikoshiASP.Controllers
{
    
    [Route("msg_buffer")]
    public class MSGBufferController : Controller
    {
        private readonly msgBuffer _mbuff;

        public MSGBufferController(msgBuffer mb)
        {
            _mbuff = mb;
            if (_mbuff.text == null) { _mbuff.text = new List<string>() { "", "N:Empty", "Chara:404" }; }
        }


        [HttpGet]
        public IActionResult Get()
        {
            Dictionary<string, object> buffer = new Dictionary<string, object>
            {
                { "text", _mbuff.text},
                { "br", _mbuff.br},
                { "hm", _mbuff.hm }
            };
            //string json = JsonSerializer.Serialize(buffer, new JsonSerializerOptions
            //{
            //    WriteIndented = true
            //});

            //Console.WriteLine("JSON Structure:");
            //Console.WriteLine(jsonson);
            return Ok(buffer);
        }
    }
}

