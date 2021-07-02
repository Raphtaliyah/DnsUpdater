using System.Xml.Serialization;

namespace DnsUpdater.NameSilo
{
    [XmlRoot("namesilo")]
    public class ApiResponse
    {
        /// <summary>
        /// Request details
        /// </summary>
        [XmlElement("request")]
        public RequestInfo Request { get; set; }
        
        /// <summary>
        /// The content received
        /// </summary>
        [XmlElement("reply")]
        public Reply Reply { get; set; }
    }
}