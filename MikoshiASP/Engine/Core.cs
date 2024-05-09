using System;
using System.Text.Json;
using System.IO;
using System.Xml.Linq;
using MikoshiASP.Controllers.Structures;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MikoshiASP.Engine
{
	public class Core : Memory,ConnectionHandler
	{

        private AKeyHandler _API;
        private string _APIKEY; //todo: create endpoint for handling api keys
        string apiUrl = "https://openrouter.ai/api/v1/chat/completions";

        public Core(string api="nokey")
        {
            _APIKEY = api;
        }

        public async Task<string> local_api()
        {
            throw new NotImplementedException();
        }

        public string? open_json(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    string fileStr;

                    using (StreamReader reader = new StreamReader(path))
                    {
                        fileStr = reader.ReadToEnd();
                    }
                    return fileStr;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }


        public async Task<string> open_router_api(string data)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_APIKEY}");

                var httpContent = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Received JSON response:");
                    Console.WriteLine(jsonResponse);

                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                    {
                        JsonElement root = doc.RootElement;

                        JsonElement contentElement = root
                            .GetProperty("choices")[0] // Access first element of "choices" array
                            .GetProperty("message")   // Access "message" property
                            .GetProperty("content");  // Access "content" property

                        if (contentElement.ValueKind == JsonValueKind.String)
                        {
                            string content = contentElement.GetString();
                            //Console.WriteLine("Extracted content:");
                            //Console.WriteLine(content); // Print the extracted content

                            return content;
                        }
                        else
                        {
                            Console.WriteLine("Content is not a string value.");
                            return "Error: Content is not a string value";
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"HTTP request failed with status code {response.StatusCode}");

                    string errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error response content: {errorResponse}");
                    return "Error";
                }
            }
        }
    

        public async Task<string> prompt_builder(string text1, double temp, double topp, double fp, double pp, string chr, string model)
        {
            try
            {
                string userContent = string.Join("\n", open_json($"./json_{chr}/brain.json"));

                List<Dictionary<string, string>> promptList = new List<Dictionary<string, string>>();


                foreach (string line in userContent.Split('\n'))
                {
                    if (line != " " || line != "")
                    {

                        string role = line.Contains("N:") ? "user" : "assistant";
                        string content = line;
                        promptList.Add(new Dictionary<string, string> { { "role", role }, { "content", content } });
                    }
                }

                // Add user prompt to the end of promptList
                promptList.Add(new Dictionary<string, string> { { "role", "user" }, { "content", "N: " + text1 } });

                // Trim promptList to the last 12 elements
                int cut = Math.Max(0, promptList.Count - 12);
                promptList = promptList.Skip(cut).ToList();

                promptList.Add(new Dictionary<string, string>
                    {
                        { "role", "system" },
                        { "content", string.Join("\n", open_json($"./json_{chr}/high_memory.json")) }
                    });

                Dictionary<string, object> finalPrompt = new Dictionary<string, object>
                {
                    { "model", model },
                    { "messages", promptList },
                    { "max_token", 300 },
                    { "temperature", temp },
                    { "frequency_penalty", fp },
                    { "presence_penalty", pp }
                };

                string jsonRequest = JsonSerializer.Serialize(finalPrompt, new JsonSerializerOptions { WriteIndented = true });
                Console.WriteLine("JSON Structure:");
                Console.WriteLine(jsonRequest);


                switch (model)
                {
                    //case "Mistral":
                    //    local_api();
                    //    break;
                    default:
                        return await open_router_api(jsonRequest);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return "Error";
            }
        }

        //high memory is a file with persistent information about current dialogue,that model will consider as behavioral prompt

        public void save_json(string data, string path, byte b = 0)
        {
            try
            {
                using (FileStream writer = new FileStream(path,FileMode.OpenOrCreate,FileAccess.ReadWrite))
                {
                    writer.Write(Encoding.ASCII.GetBytes(data),0,data.Length);
                }

                Console.WriteLine("Text saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}

