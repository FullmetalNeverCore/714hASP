using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MikoshiASP.Controllers.Structures;
using MikoshiASP.Engine;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MikoshiASP.Controllers
{
    [Route("chat_exchange")]
    public class ChatExchangeController : Controller
    {
        private readonly msgBuffer _mbuff;
        private readonly Model _model;
        private string _memoryplusnew;
        private Core _core;

        public ChatExchangeController(msgBuffer mb,Model model,AKeyHandler api)
        {
            _mbuff = mb;
            _core = new Core(api.API_KEY);
            _model = model;
        }

        [HttpPost]
        public async Task Post([FromBody] chatEx value)
        {
            string chat,answ;
            
            Console.WriteLine("chatExchange");
            Console.WriteLine(_model.chr);
            Console.WriteLine(value.chat);
            Console.WriteLine(value.type);
            switch (value.type)
            {
                case "Mistral":
                    chat = $"{value.chat} {System.Environment.NewLine} {_model.chr}:";
                    Console.WriteLine("Not Implemented");
                    break;
                default:
                    chat = $"{value.chat} {System.Environment.NewLine} {_model.chr}:";
                    answ = await _core.prompt_builder(chat, Double.Parse(_model.temp), 1.0, Double.Parse(_model.fpen), Double.Parse(_model.ppen), _model.chr, value.type);
                    Console.WriteLine(answ);
                    _mbuff.text = new List<string>() { "", $"N:{value.chat}", $"{_model.chr}:{answ}" };
                    Console.WriteLine($"N:{value.chat} {_model.chr}:{answ}");
                    string previousMemory = _core.open_json($"json_{_model.chr}/brain.json");
                    _memoryplusnew = $"{previousMemory} N:{value.chat} {System.Environment.NewLine} {_model.chr}:{answ} {System.Environment.NewLine}";
                    _core.save_json(_memoryplusnew, $"json_{_model.chr}/brain.json");
                    _mbuff.br = _core.open_json($"./json_{_model.chr}/brain.json");
                    _mbuff.hm = _core.open_json($"./json_{_model.chr}/high_memory.json");
                    break;
            }
            
        }

    }
}

