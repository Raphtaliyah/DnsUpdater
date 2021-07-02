using System.Collections.Generic;

namespace DnsUpdater
{
    public class Config
    {
        public const string NEW_CONFIG_APIKEY = "NAMESILO API KEY HERE";
        
        /// <summary>
        /// Api key provided by NameSilo
        /// </summary>
        public string ApiKey { get; }
        /// <summary>
        /// Refresh interval in seconds
        /// </summary>
        public int IntervalSeconds { get; }
        /// <summary>
        /// Domains and subdomains
        /// </summary>
        public Dictionary<string, List<string>> Domains { get; }

        public Config(string apiKey = NEW_CONFIG_APIKEY, int intervalSeconds = 300, Dictionary<string, List<string>> domains = null)
        {
            ApiKey = apiKey;
            IntervalSeconds = intervalSeconds;
            Domains = domains ?? new Dictionary<string, List<string>>()
            {
                {
                    "example.com", new List<string>()
                    {
                        ""
                    }
                }
            };
        }
    }
}