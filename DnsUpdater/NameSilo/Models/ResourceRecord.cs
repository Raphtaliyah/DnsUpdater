using System.Xml.Serialization;

namespace DnsUpdater.NameSilo.Models
{
    public class ResourceRecord
    { 
        /// <summary>
        /// The unique id of this dns record
        /// </summary>
        [XmlElement("record_id")]
        public string RecordId { get; set; }
        /// <summary>
        /// The type of <paramref name="value"/>
        /// </summary>
        [XmlElement("type")]
        public RecordType Type { get; set;  }
        /// <summary>
        /// The host the record belongs to
        /// </summary>
        [XmlElement("host")]
        public string Host { get; set; }
        /// <summary>
        /// The value of the record
        /// </summary>
        [XmlElement("value")]
        public string Value { get; set; }
        /// <summary>
        /// The TTL for this record (default is 7207 if not provided)
        /// </summary>
        [XmlElement("ttl")]
        public int Ttl { get; set; } = 7207;
        /// <summary>
        /// Only used for MX (default is 10 if not provided)
        /// </summary>
        [XmlElement("distance")] 
        public int Distance { get; set; } = 10;
    }

    public enum RecordType
    {
        /// <summary>
        /// IPv4 address
        /// </summary>
        A,
        /// <summary>
        /// IPv6 address
        /// </summary>
        AAAA,
        /// <summary>
        /// Canonical name
        /// </summary>
        CNAME,
        /// <summary>
        /// Mail exchange
        /// </summary>
        MX,
        /// <summary>
        /// Text
        /// </summary>
        TXT,
    }
}