using System;
using System.Xml.Serialization;

namespace Mews.Eet.Dto.Wsdl
{
    [Serializable]
    [XmlType(Namespace = "http://fs.gov.cz/eet/schema/v4")]
    public enum SignatureCipherType
    {
        [XmlEnum("RSA2048")]
        Rsa2048
    }
}
