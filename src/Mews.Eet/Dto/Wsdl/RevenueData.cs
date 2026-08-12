using System;
using System.Xml.Serialization;

namespace Mews.Eet.Dto.Wsdl
{
    [Serializable]
    [XmlType(Namespace = "http://fs.gov.cz/eet/schema/v4")]
    public class RevenueData
    {
        [XmlAttribute(AttributeName = "eic_popl")]
        public string TaxPayerTaxIdentifier { get; set; }

        [XmlAttribute(AttributeName = "eic_poverujiciho")]
        public string MandantingTaxPayerIdentifier { get; set; }
        [XmlIgnore]
        public bool MandantingTaxPayerIdentifierSpecified { get; set; }

        [XmlAttribute(AttributeName = "povereni_vice_popl")]
        public bool MandatedByMultipleTaxPayers { get; set; }
        [XmlIgnore]
        public bool MandatedByMultipleTaxPayersSpecified { get; set; }

        [XmlAttribute(AttributeName = "id_jednotky")]
        public int RegistrationUnitIdentifier { get; set; }

        [XmlAttribute(AttributeName = "id_pokl")]
        public string RegistryIdentifier { get; set; }

        [XmlAttribute(AttributeName = "porad_cis")]
        public string BillNumber { get; set; }

        [XmlAttribute(AttributeName = "dat_trzby")]
        public DateTime Accepted { get; set; }

        [XmlAttribute(AttributeName = "celk_trzba")]
        public decimal Total { get; set; }

        [XmlAttribute(AttributeName = "urceno_cerp_zuct")]
        public decimal Deposit { get; set; }
        [XmlIgnore]
        public bool DepositSpecified { get; set; }

        [XmlAttribute(AttributeName = "cerp_zuct")]
        public decimal DepositUsed { get; set; }
        [XmlIgnore]
        public bool DepositUsedSpecified { get; set; }
    }
}
