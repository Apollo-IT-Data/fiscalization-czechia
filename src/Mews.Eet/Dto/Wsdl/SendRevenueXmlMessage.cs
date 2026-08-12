using System.Xml.Serialization;

namespace Mews.Eet.Dto.Wsdl
{
    [XmlRoot(ElementName = "Trzba", Namespace = "http://fs.gov.cz/eet/schema/v4")]
    public class SendRevenueXmlMessage
    {
        [XmlElement(Namespace = "http://fs.gov.cz/eet/schema/v4", Order = 0, ElementName = "Hlavicka")]
        public RevenueHeader Header { get; set; }

        [XmlElement(Namespace = "http://fs.gov.cz/eet/schema/v4", Order = 1)]
        public RevenueData Data { get; set; }
    }
}
