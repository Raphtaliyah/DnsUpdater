using System;
using System.IO;
using System.Threading.Tasks;
using DnsUpdater.Extensions;
using Newtonsoft.Json;

namespace DnsUpdater
{
    class Program
    {
        //TODO: From args?
        private const string CONFIG_FILE = "Config.json";
        
        static async Task Main(string[] args)
        {
            var logger = Logger.Get();
            try
            {
                logger.LogInfo("Starting NameSilo DNS updater.");
                
                var config = await LoadConfigAsync(CONFIG_FILE); 
                var updater = new DnsUpdater(config);

                await updater.StartUpdaterAsync();
            }
            catch (Exception e)
            {
                logger.LogError("Something went wrong!", e);
            }
        }
        
        /// <summary>
        /// Loads the configuration from a file
        /// </summary>
        /// <param name="configFile">The path to load the file from</param>
        /// <returns>The configuration object or null, if loading failed</returns>
        private static async ValueTask<Config> LoadConfigAsync(string configFile)
        {
            var logger = Logger.Get();

            if (!File.Exists(configFile))
            {
                logger.LogInfo($"Config file not found, creating!");
                await File.WriteAllTextAsync(configFile, 
                    JsonConvert.SerializeObject(new Config(), Formatting.Indented));
            }
            
            logger.LogInfo($"Using config file: '{configFile}'.");

            var configContent = await File.ReadAllTextAsync(configFile);
            var config = JsonConvert.DeserializeObject<Config>(configContent);

            if (config == null)
            {
                logger.LogError("Invalid config file!");
                Environment.Exit(0);
            }

            if (config.ApiKey == Config.NEW_CONFIG_APIKEY)
            {
                logger.LogInfo($"Edit your config file! It's here: {configFile}");
                Environment.Exit(0);
            }

            return config;
        }
    }
}