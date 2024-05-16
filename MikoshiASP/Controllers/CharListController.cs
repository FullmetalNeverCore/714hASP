using System;
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

namespace MikoshiASP.Controllers
{
    [Route("char_list")]
    [ApiController]
    public class CharListController : Controller
    {
        private readonly ILogger<CharListController> _logger;

        public CharListController(ILogger<CharListController> logger)
        {
            _logger = logger;
        }

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
                _logger.LogError(ex, "Error while getting folder names from {rootDir}", rootDir);
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
                IMG img = JsonSerializer.Deserialize<IMG>(jsonString);

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
                                    _logger.LogInformation("Interface: {netInterfaceName}, IP Address: {ipAddress}", netInterface.Name, ip.Address);
                                    _logger.LogInformation("Local IP Address: {ipAddress}", ip.ToString());
                                    if (img.link.Contains("192"))
                                    {
                                        _logger.LogInformation("Old local ip detected");
                                        _logger.LogInformation("http://{ipAddress}:5005/{path}", ip.Address, parentDir.Replace("./", "").Replace("json_", ""));
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
                    _logger.LogError(ex, "Error while processing network interfaces");
                }
            }
            catch (FileNotFoundException ex)
            {
                Core.save_json($"charlist: {ex.Message}", "./error.json");
                _logger.LogError(ex, "File not found: {parentDir}/img.json", parentDir);
            }
            catch (JsonException ex)
            {
                Core.save_json($"charlist: {ex.Message}", "./error.json");
                _logger.LogError(ex, "JSON deserialization error in {parentDir}/img.json", parentDir);
            }

            return link;
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
