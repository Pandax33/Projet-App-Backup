using System;
using System.Text.Json;
using System.IO;

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
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(obj, options);
            File.WriteAllText(JsonPath, json);
        }

        public T Deserialize<T>()
        {
            if (!File.Exists(JsonPath))
            {
                throw new FileNotFoundException($"Le fichier {JsonPath} n'existe pas.");
            }

            string json = File.ReadAllText(JsonPath);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}