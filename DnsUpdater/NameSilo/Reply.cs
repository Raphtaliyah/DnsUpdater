using System;
using System.Xml.Serialization;
using DnsUpdater.NameSilo.Models;

namespace DnsUpdater.NameSilo
{
    public class Reply
    {
        /// <summary>
        /// Status code
        /// </summary>
        [XmlElement("code")]
        public int Code { get; set; }
        
        /// <summary>
        /// The status code detailed
        /// </summary>
        [XmlElement("detail")]
        public string Detail { get; set; }
        
        //TODO: There has to be a better way
        /// <summary>
        /// The content of the response
        /// </summary>
        [XmlElement(ElementName = "record_id", Type = typeof(string))]
        [XmlElement(ElementName = "resource_record", Type = typeof(ResourceRecord))]
        public object[] Content { get; set; }
        
        /// <summary>
        /// Returns the content cast to a different type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] GetContents<T>() => Content == null ? null : Array.ConvertAll(Content, item => (T) item);
    }
}