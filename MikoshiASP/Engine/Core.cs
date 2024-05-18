using System;
using System.Text.Json;
using System.IO;
using System.Xml.Linq;
using MikoshiASP.Controllers.Structures;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MikoshiASP.Controllers.Misc;

namespace MikoshiASP.Engine
{
	public class Core : ConnectionHandler
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

        public static string? open_json(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch(Exception ex)
            {
                LoggingErrors.LogErr(ex.Message);
            }

            return null;
        }


        public async Task<string> open_router_api(string data)
        {
            try
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
                        LoggingErrors.LogErr(errorResponse);
                        return "Error";
                    }
                }
            }
            catch(Exception ex)
            {
                LoggingErrors.LogErr(ex.Message);
                return "Error";
            }
            
        }
    

        public async Task<string> prompt_builder(string text1, double temp, double topp, double fp, double pp, string chr, string model)
        {
            try
            {
                string userContent = open_json($"./json_{chr}/brain.json");

                List<Dictionary<string, string>> promptList = new List<Dictionary<string, string>>();

                promptList.Add(new Dictionary<string, string>
                    {
                        { "role", "system" },
                        { "content",open_json($"./json_{chr}/high_memory.json") }
                    });

                foreach (string line in userContent.Split('\n'))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {

                        string role = line.Contains("N:") ? "user" : "assistant";
                        string content = line;
                        promptList.Add(new Dictionary<string, string> { { "role", role }, { "content", content } });
                    }
                }

                promptList.Add(new Dictionary<string, string> { { "role", "user" }, { "content", "N: " + text1 } });



                Dictionary<string, object> finalPrompt = new Dictionary<string, object>
                {
                    { "model", model },
                    { "messages", promptList },
                    { "max_token", 150 },
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
                    //    break;321
                    default:
                        return await open_router_api(jsonRequest);
                }

            }
            catch (Exception ex)
            {
                LoggingErrors.LogErr(ex.Message);
                return "Error";
            }
        }

        //high memory is a file with persistent information about current dialogue,that model will consider as behavioral prompt

        public static void save_json(string data, string path)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.Write(data);
                }

                Console.WriteLine("Text saved to file successfully.");
            }
            catch (Exception ex)
            {
                LoggingErrors.LogErr(ex.Message);
            }
        }
    }
}

