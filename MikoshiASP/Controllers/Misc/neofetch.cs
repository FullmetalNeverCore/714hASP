using System;
using System.Diagnostics;

namespace MikoshiASP.Controllers.Misc
{
	public static class neofetch
	{
        public static string MachineName = Environment.MachineName;

        public static string HostOS = Environment.OSVersion.Platform == PlatformID.Unix ? "MikoshiASP from Linux with Love" : "MikoshiASP from Windows with Love";


        public static async Task<string> CPUUsageAsync()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "sh",
                        Arguments = "-c \"top -bn2 | grep 'Cpu(s)' | tail -n 1 | awk '{print $2 + $4}'\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false
                    }
                };

                process.Start();

                string output = await process.StandardOutput.ReadToEndAsync();
                process.WaitForExit();

                return $"CPU USAGE: {output.Trim()}%";

            }
            catch (Exception ex)
            {
                return "Cannot obtain CPU usage";
            }
        }



        //currently memory information available only for linux based systems.
        public static async Task<string> GetRamUsageAsync()
        {
            string output;
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "free",
                        Arguments = "-m",
                        RedirectStandardOutput = true,
                        UseShellExecute = false
                    }
                };

                process.Start();

                output = await process.StandardOutput.ReadToEndAsync();
                process.WaitForExit();

                var lines = output.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var memoryLine = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var totalMemory = memoryLine[1];
                var freeMemory = memoryLine[3];

                return $"{freeMemory}/{totalMemory} mb. available RAM";
            }
            catch (PlatformNotSupportedException pns)
            {
                output = "Not available. Platform not supported!";
            }
            catch (Exception ex)
            {
                output = $"Not available. {ex.Message}";
            }
            return output;
		}

        public static async Task<string> get_stat()
        {
            string output = @$"
                    {neofetch.MachineName}

                    --------------------
                    {await neofetch.CPUUsageAsync()}
                    {await neofetch.GetRamUsageAsync()}

                    {neofetch.HostOS}

            ";
            return output; 
        }
	}
}

