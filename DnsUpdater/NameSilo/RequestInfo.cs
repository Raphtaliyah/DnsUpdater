using System.Xml.Serialization;

namespace DnsUpdater.NameSilo
{
    public class RequestInfo
    {
        /// <summary>
        /// The operation that was performed
        /// </summary>
        [XmlElement("operation")]
        public string Operation { get; set; }
        
        /// <summary>
        /// The public ipv4 address of this machine
        /// </summary>
        [XmlElement("ip")]
        public string IpAddress { get; set; }
    }
}