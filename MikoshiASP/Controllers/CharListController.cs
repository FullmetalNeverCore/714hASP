using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using MikoshiASP.Controllers.Structures;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using MikoshiASP.Engine;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MikoshiASP.Controllers
{

    [Route("char_list")]
    [ApiController]
    public class CharListController : Controller
    {
        private List<string> _GetFolderNames(string rootDir)
        {
            List<string> folderNames = new List<string>();

            try
            {
                DirectoryInfo dirInf = new DirectoryInfo(rootDir);

                DirectoryInfo[] listOfdirs = dirInf.GetDirectories();

                foreach (var dir in listOfdirs)
                {
                    if (dir.Name.Contains("json_"))
                    {
                        folderNames.Add(dir.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                Core.save_json($"charlist: {ex.Message}", "./error.json");
            }
            return folderNames;
        }

        private List<string> _GetImages(string parentDir)
        {
            List<string> link = new List<string>();
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

            try
            {
                string jsonString = System.IO.File.ReadAllText($"{parentDir}/img.json");

                IMG img= JsonSerializer.Deserialize<IMG>(jsonString);

                try
                {
                    NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                    foreach (NetworkInterface netInterface in networkInterfaces)
                    {
                        if (netInterface.OperationalStatus == OperationalStatus.Up &&
                            netInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                            netInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                        {
                            IPInterfaceProperties ipProps = netInterface.GetIPProperties();

                            foreach (UnicastIPAddressInformation ip in ipProps.UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
                                    !IPAddress.IsLoopback(ip.Address))
                                {
                                    Console.WriteLine($"Interface: {netInterface.Name}, IP Address: {ip.Address}");

                                    Console.WriteLine("Local IP Address: " + ip.ToString());
                                    if (img.link.Contains("192"))
                                    {
                                        Console.WriteLine("Old local ip detected");
                                        Console.WriteLine($"http://{ip.Address}:5005/{parentDir.Replace("./", "").Replace("json_", "")}");
                                        link.Add($"http://{ip.Address}:5005/{parentDir.Replace("./", "").Replace("json_", "")}");
                                    }
                                    else
                                    {
                                        link.Add(img.link);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Core.save_json($"charlist: {ex.Message}", "./error.json");
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }


            }
            catch (FileNotFoundException ex)
            {
                Core.save_json($"charlist: {ex.Message}", "./error.json");
            }
            catch (JsonException ex)
            {
                Core.save_json($"charlist: {ex.Message}", "./error.json");
            }

            return link;
        }

        private readonly ILogger<CharListController> _logger;

        public CharListController(ILogger<CharListController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Get()
        {
            List<string> names = _GetFolderNames("./");

            Dictionary<string, string> namelink = new Dictionary<string, string>();

            foreach (var name in names)
            {
                string cleanName = name.Replace("json_", "");
                List<string> imagePaths = _GetImages($"./{name}");


                namelink.Add(cleanName, imagePaths[0]);
            }

            return Ok(namelink); 
        }
    }
}

