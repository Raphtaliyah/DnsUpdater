using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnsUpdater.Extensions;
using DnsUpdater.NameSilo;
using DnsUpdater.NameSilo.Models;

namespace DnsUpdater
{
    public class DnsUpdater
    {
        private readonly Logger _logger;
        private readonly Config _config;
        private readonly NameSiloApi _api;
        private bool _previousFailed;
        private string _lastIp;
        
        public DnsUpdater(Config config)
        {
            _logger = Logger.Get();
            _config = config;
            _api = new NameSiloApi(config);
            _lastIp = "First update cycle will always update!";
            _logger.LogInfo($"Created dns updater for {config.Domains.Count} domain(s).");
        }
        
        /// <summary>
        /// Starts the updater and blocks indefinitely
        /// </summary>
        public async Task StartUpdaterAsync()
        {
            while (true)
            {
                try
                {
                    await BeginUpdateCycle();
                }
                catch (Exception e)
                {
                    _logger.LogError("Something went wrong while performing an update cycle!", e);
                }
                
                await Task.Delay(_config.IntervalSeconds * 1000);
            }
            // ReSharper disable once FunctionNeverReturns
        }
        private async Task BeginUpdateCycle()
        {
            _logger.LogInfo("Beginning update cycle...");
            var willUpdate = false;
            foreach (var (domain, subDomains) in _config.Domains)
            {
                if (subDomains.Count == 0)
                {
                    _logger.LogWarning($"No subdomains specified for '{domain}'. " +
                                       $"To update the naked domain add an empty string!");
                    continue;
                }
                
                //Get the dns records
                var dnsRecordsResponse = (await _api.ListDnsRecordsAsync(domain)).WarningOnFail();
                if (dnsRecordsResponse.Reply.Code != NameSiloApi.API_SUCCESS)
                {
                    _logger.LogError($"Failed to get dns records for '{domain}'! Won't be updated!");
                    _previousFailed = true;
                    continue;
                }
                    
                //Check and update ip address
                var newIp = dnsRecordsResponse.Request.IpAddress;
                if (!_previousFailed && !willUpdate && _lastIp == newIp)
                {
                    _logger.LogInfo($"No change in ip address detected. Current: {_lastIp}");
                    break;
                }
                
                //Write to the log about a new about cycle happening
                if (!willUpdate) 
                    _logger.LogInfo(_previousFailed
                        ? $"The previous about cycle had some problems, forcing an update. Ip then: {_lastIp} | Ip now: {newIp}"
                        : $"New ip address detected: {newIp} | Old: {_lastIp}");
                
                //_lastIp will be equal to newIp for all after the first even if the ip changed, this will force an update
                willUpdate = true;
                //clear the fault
                _previousFailed = false;
                _lastIp = newIp;
                
                //Update subdomain ips
                var dnsRecords = dnsRecordsResponse.Reply.GetContents<ResourceRecord>();
                await UpdateSubDomains(domain, subDomains, dnsRecords, newIp);
            }
            _logger.LogInfo($"Everything updated{(_previousFailed ? " with faults" : string.Empty)}! See you in {_config.IntervalSeconds} seconds!");
        }
        private async Task UpdateSubDomains(string domain, List<string> subDomains, ResourceRecord[] dnsRecords, string ipAddress)
        {
            foreach (var subDomain in subDomains)
            {
                var fullDomain = $"{(string.IsNullOrEmpty(subDomain) ? string.Empty : $"{subDomain}.")}{domain}";
                
                var potentialDnsRecords = dnsRecords?
                    .Where(x => x.Type == RecordType.A && 
                                x.Host == $"{fullDomain}").ToList() ?? new List<ResourceRecord>(1);
                
                //If there are more records, there is no way to know which one to update.
                //You shouldn't use more A records if you want dynamic dns anyways
                if (potentialDnsRecords.Count > 1)
                {
                    _logger.LogWarning($"Full domain '{fullDomain}' has multiple A records! " +
                                      $"Please only leave one and remove the rest. Won't be updated!");
                    _previousFailed = true;
                    continue;
                }
                
                if (potentialDnsRecords.Count == 0)
                {
                    _logger.LogInfo($"'{fullDomain}' doesn't have any A records, creating one!");

                    var newRecord = new ResourceRecord()
                    {
                        Type = RecordType.A,
                        Host = subDomain,
                        Value = ipAddress
                    };
                    var createResponse = (await _api.CreateDnsRecordAsync(domain, newRecord)).WarningOnFail();
                    if (createResponse.Reply.Code != NameSiloApi.API_SUCCESS)
                        continue;
                    
                    potentialDnsRecords.Add(newRecord);
                }
                else
                {
                    potentialDnsRecords[0].Value = ipAddress;
                    var updateResponse = (await _api.UpdateDnsRecordAsync(domain, potentialDnsRecords[0])).WarningOnFail();
                    if (updateResponse.Reply.Code != NameSiloApi.API_SUCCESS)
                        continue;
                }

                _logger.LogInfo($"Updated {fullDomain} to {ipAddress}!");
            }
        }
    }
}