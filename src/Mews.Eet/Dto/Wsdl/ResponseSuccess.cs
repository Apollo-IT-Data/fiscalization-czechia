using System;
using System.Xml.Serialization;

namespace Mews.Eet.Dto.Wsdl
{
    [Serializable]
    [XmlType(Namespace = "http://fs.gov.cz/eet/schema/v4")]
    public class ResponseSuccess
    {
        [XmlAttribute(AttributeName = "pok")]
        public string ConfirmationCode { get; set; }

        [XmlAttribute(AttributeName = "test")]
        public bool IsPlayground { get; set; }

        [XmlIgnore]
        public bool IsPlaygroundSpecified { get; set; }
    }
}
