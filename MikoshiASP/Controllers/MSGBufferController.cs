using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MikoshiASP.Controllers.Structures;

namespace MikoshiASP.Controllers
{
    [Route("msg_buffer")]
    public class MSGBufferController : Controller
    {
        private readonly msgBuffer _mbuff;
        private readonly ILogger<MSGBufferController> _logger;

        public MSGBufferController(msgBuffer mb, ILogger<MSGBufferController> logger)
        {
            _mbuff = mb;
            _logger = logger;

            if (_mbuff.text == null)
            {
                _mbuff.text = new List<string> { "", "N:Empty", "Chara:404" };
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            Dictionary<string, object> buffer = new Dictionary<string, object>
            {
                { "text", _mbuff.text },
                { "br", _mbuff.br },
                { "hm", _mbuff.hm }
            };

            string json = JsonSerializer.Serialize(buffer, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            _logger.LogInformation("JSON Structure: {json}", json);

            return Ok(buffer);
        }
    }
}
