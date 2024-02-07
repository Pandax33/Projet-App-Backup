using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ProjetDevSys
{
    public static class AppConstants
    {
        // Remplacez par une propriété statique en lecture seule plutôt qu'une constante
        public static readonly string LogFilePath;
        public static readonly string Langage;

        // Bloc statique pour initialiser les propriétés statiques
        static AppConstants()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            // Assignez la valeur depuis appsettings.json à la propriété statique
            LogFilePath = configuration["Logging:JsonPath"];
            Langage = configuration["Langage:Langage"];
            
        }
    }
}