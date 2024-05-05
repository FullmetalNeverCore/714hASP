using System;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
namespace MikoshiASP.Controllers.Structures

{
	public class EngineUpdate
	{
		//data: {username: username, password: password, char: char},

		public string username { get; set; }
		public string password { get; set; }
        [JsonPropertyName("char")]
        public string chara { get; set; }
	}
}

