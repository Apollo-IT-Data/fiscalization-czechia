using System;
using System.Xml.Serialization;

namespace Mews.Eet.Dto.Wsdl
{
    [XmlRoot(ElementName = "Hlavicka", Namespace = "http://fs.gov.cz/eet/schema/v4")]
    public class RevenueHeader
    {
        [XmlAttribute(AttributeName = "uuid_zpravy")]
        public string MessageUuid { get; set; }

        [XmlAttribute(AttributeName = "dat_odesl")]
        public DateTime Sent { get; set; }

        [XmlAttribute(AttributeName = "prvni_zaslani")]
        public bool FirstTry { get; set; }

        [XmlAttribute(AttributeName = "overeni")]
        public bool Verification { get; set; }
        [XmlIgnore]
        public bool VerificationSpecified { get; set; }
    }
}
