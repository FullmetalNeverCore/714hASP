using System;
namespace MikoshiASP.Engine
{
	public interface ConnectionHandler
	{
        Task<string> prompt_builder(string text1, double temp, double topp, double fp, double pp, string chr, string model);

        Task<string> local_api();

        Task<string> open_router_api(string data);
	}
}

