using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DnsUpdater.NameSilo.Models;

namespace DnsUpdater.NameSilo
{
    public class NameSiloApi
    {
        private const int API_VERSION = 1;
        private const string RESPONSE_TYPE = "xml";
        private const string ENDPOINT = "https://namesilo.com/api";
        private const string DNS_LIST_METHOD = "dnsListRecords";
        private const string DNS_UPDATE_METHOD = "dnsUpdateRecord";
        private const string DNS_ADD_METHOD = "dnsAddRecord";

        public const int API_SUCCESS = 300;

        private readonly XmlSerializer _serializer;
        private readonly HttpClient _client;
        private readonly Config _config;

        public NameSiloApi(Config config)
        {
            _serializer = new XmlSerializer(typeof(ApiResponse));
            _config = config;
            _client = new HttpClient();
        }

        public async Task<ApiResponse> ListDnsRecordsAsync(string domain)
        {
            var resp = await _client.GetStreamAsync(GetEndpoint(DNS_LIST_METHOD, $"domain={domain}"));
            
            return _serializer.Deserialize(resp) as ApiResponse;
        }
        
        public async Task<ApiResponse> UpdateDnsRecordAsync(string domain, ResourceRecord record)
        {
            var resp = await _client.GetStreamAsync(GetEndpoint(DNS_UPDATE_METHOD, 
                $"domain={domain}&" +
                $"rrid={record.RecordId}&" +
                $"rrhost={HostToApiFormat(domain, record.Host)}&" +
                $"rrvalue={record.Value}&" +
                $"rrdistance={record.Distance}&" +
                $"rttl={record.Ttl}"));
            
            return _serializer.Deserialize(resp) as ApiResponse;
        }

        public async Task<ApiResponse> CreateDnsRecordAsync(string domain, ResourceRecord record)
        {
            var resp = await _client.GetStreamAsync(GetEndpoint(DNS_ADD_METHOD, 
                $"domain={domain}&" +
                $"rrtype={record.Type}&" +
                $"rrhost={HostToApiFormat(domain, record.Host)}&" +
                $"rrvalue={record.Value}&" +
                $"rrdistance={record.Distance}&" +
                $"rttl={record.Ttl}"));
            
            return _serializer.Deserialize(resp) as ApiResponse;
        }

        private string GetEndpoint(string method, string parameters) => 
            $"{ENDPOINT}/{method}?version={API_VERSION}&type={RESPONSE_TYPE}&key={_config.ApiKey}&{parameters}";
        private static string HostToApiFormat(string domain, string fullDomain)
        {
            //This is ugly, but performance isn't an issue anyways
            return fullDomain.Replace($".{domain}", string.Empty)
                             .Replace(domain, string.Empty);
        }
    }
}