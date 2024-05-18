using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MikoshiASP.Controllers.Misc;
using MikoshiASP.Controllers.Structures;
using MikoshiASP.Engine;

namespace MikoshiASP.Controllers
{
    [Route("chat_exchange")]
    public class ChatExchangeController : Controller
    {
        private readonly msgBuffer _mbuff;
        private readonly Model _model;
        private string _memoryplusnew;
        private readonly Core _core;
        private readonly ILogger<ChatExchangeController> _logger;

        string brain;
        string hmemory;

        public ChatExchangeController(msgBuffer mb, Model model, AKeyHandler api, ILogger<ChatExchangeController> logger)
        {
            _mbuff = mb;
            _core = new Core(api:api.API_KEY);
            _model = model;
            _logger = logger;
            brain = $"json_{_model.chr}/ brain.json";
            hmemory = $"./json_{_model.chr}/high_memory.json";
        }


        private async Task HandleInteraction(chatEx value)
        {
            string chat, answ;

            chat = $"{value.chat} {Environment.NewLine} {_model.chr}:";
            answ = await _core.prompt_builder(chat, double.Parse(_model.temp), 1.0, double.Parse(_model.fpen), double.Parse(_model.ppen), _model.chr, value.type);

            _logger.LogInformation("Answer: {answ}", answ);
            _mbuff.text = new List<string> { "", $"N:{value.chat}", $"{_model.chr}:{answ}" };
            _logger.LogInformation($"N:{value.chat} {_model.chr}:{answ}");

            string previousMemory = Core.open_json(brain);
            _memoryplusnew = $"{previousMemory} N:{value.chat} {Environment.NewLine} {_model.chr}:{answ} {Environment.NewLine}";
            Core.save_json(_memoryplusnew, brain);

            _mbuff.br = Core.open_json(brain);
            _mbuff.hm = Core.open_json(hmemory);
        }

        // TODO: Create an endpoint for hard reset of memory
        [HttpPost]
        public async Task Post([FromBody] chatEx value)
        {
            

            _logger.LogInformation("chatExchange");
            _logger.LogInformation("Character: {chr}", _model.chr);
            _logger.LogInformation("Chat: {chat}", value.chat);
            _logger.LogInformation("Type: {type}", value.type);

            try
            {
                switch (value.type)
                {
                    case "Mistral":
                        _logger.LogInformation("Mistral type is not implemented");
                        break;
                    default:
                        await HandleInteraction(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggingErrors.LogErr(ex.Message);
                _logger.LogError(ex, "An error occurred during chat exchange");
            }
        }
    }
}
