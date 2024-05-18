using System;
namespace MikoshiASP.Controllers.Misc
{
	public static class LoggingErrors
	{
		public static void LogErr(string message)
		{
			try
			{
				Console.WriteLine(message);
				File.WriteAllText("./error.json", message);
			}
			catch(Exception ex)
			{
				Console.WriteLine($"info : {ex}");
			}
		}
	}
}

