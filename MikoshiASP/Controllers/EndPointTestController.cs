using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MikoshiASP.Controllers.Structures;
using MikoshiASP.Engine;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace MikoshiASP.Controllers
//{
//    [Route("endpointtest")]
//    public class EndPointTestController : Controller
//    {

//        private Core _core = new Core();
//        private readonly Model _model;

//        public EndPointTestController(Model model)
//        {
//            _model = model;
//        }

//        [HttpPost]
//        public void Post([FromBody]string value)
//        {
//            string memoryplusnew = $"{_core.open_json($"./json_{_model.chr}/brain.json")}{_model.chr}:{value}{System.Environment.NewLine}";
//            _core.save_json(memoryplusnew,$"./json_{_model.chr}/brain.json");
//        }

//    }
//}

