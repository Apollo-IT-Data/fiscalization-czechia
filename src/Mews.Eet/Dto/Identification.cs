using System;
using Mews.Eet.Dto.Identifiers;

namespace Mews.Eet.Dto
{
    public class Identification
    {
        public Identification(TaxIdentifier taxPayerIdentifier, RegistryIdentifier registryIdentifier, RegistrationUnitIdentifier registrationUnitIdentifier, Certificate certificate)
            : this(taxPayerIdentifier, null, registryIdentifier, registrationUnitIdentifier, certificate)
        {
        }

        public Identification(TaxIdentifier taxPayerIdentifier, TaxIdentifier mandatingTaxPayerIdentifier, RegistryIdentifier registryIdentifier, RegistrationUnitIdentifier registrationUnitIdentifier, MandationType mandationType, Certificate certificate)
            : this(mandatingTaxPayerIdentifier, mandationType == Dto.MandationType.Section9Paragraph1 ? taxPayerIdentifier : null, registryIdentifier, registrationUnitIdentifier, certificate, mandationType)
        {
        }

        private Identification(TaxIdentifier taxPayerIdentifier, TaxIdentifier mandantingTaxPayerIdentifier, RegistryIdentifier registryIdentifier, RegistrationUnitIdentifier registrationUnitIdentifier, Certificate certificate, MandationType? mandationType = null)
        {
            if (taxPayerIdentifier == null)
            {
                throw new ArgumentException("The taxpayer identifier is required.");
            }

            if (registryIdentifier == null)
            {
                throw new ArgumentException("Registry identifier is required.");
            }

            if (registrationUnitIdentifier == null)
            {
                throw new ArgumentException("Registration unit identifier is required.");
            }

            if (certificate == null)
            {
                throw new ArgumentException("The certificate cannot be null.");
            }

            TaxPayerIdentifier = taxPayerIdentifier;
            MandantingTaxPayerIdentifier = mandantingTaxPayerIdentifier;
            RegistryIdentifier = registryIdentifier;
            RegistrationUnitIdentifier = registrationUnitIdentifier;
            Certificate = certificate;
        }

        public TaxIdentifier TaxPayerIdentifier { get; }

        public TaxIdentifier MandantingTaxPayerIdentifier { get; }

        public RegistryIdentifier RegistryIdentifier { get; }

        public RegistrationUnitIdentifier RegistrationUnitIdentifier { get; }

        public Certificate Certificate { get; }
    }
}
