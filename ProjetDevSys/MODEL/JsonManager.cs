using System;
using System.IO;
using Newtonsoft.Json;

namespace ProjetDevSys.MODEL
{
    public class JsonManager
    {
        public string JsonPath { get; set; }

        public JsonManager(string jsonPath)
        {
            JsonPath = jsonPath;
        }

        public void Serialize<T>(T obj)
        {
            // Configure Newtonsoft.Json to format the JSON file
            JsonSerializerSettings settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            string json = JsonConvert.SerializeObject(obj, settings);
            File.WriteAllText(JsonPath, json);
        }

        public T Deserialize<T>()
        {
            if (!File.Exists(JsonPath))
            {
                throw new FileNotFoundException(ResourceHelper.GetString("JsonManager1"));
            }

            string json = File.ReadAllText(JsonPath);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
