using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MikoshiASP.Controllers.Misc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MikoshiASP.Controllers
{
    [Route("neofetch")]
    public class NeofetchController : Controller
    {
        [HttpGet]
        public async  Task<IActionResult> Get()
        {
            try
            {
                string neofetchStats = await neofetch.get_stat();
                return Ok(neofetchStats);
            }
            catch
            {
                return StatusCode(500, "An error occurred while fetching system stats.");
            }
        }
    }
}

