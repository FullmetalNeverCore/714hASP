using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MikoshiASP.Engine;

namespace MikoshiASP.Controllers
{
    [Route("api/errormsg")]
    [ApiController]
    public class ErrorLinkController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            //todo : add ram consumption to errmsg information,and other host info .
            try
            {
                // Obtain the last error message from source 
                string errorMessage = Core.open_json("./error.json");
                Console.WriteLine($"Last Exception: {errorMessage}");

                Dictionary<string, string> response = new Dictionary<string, string>
                {
                    { "err", errorMessage }
                };
                Console.WriteLine(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during processing
                Core.save_json($"api/errormsg : {ex.Message}", "./error.json");
                return BadRequest(ex.Message);
            }
        }
    }
}
