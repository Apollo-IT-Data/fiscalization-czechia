using Mews.Eet.Converters;
using Mews.Eet.Dto;
using Mews.Eet.Dto.Wsdl;
using Mews.Eet.Extensions;

namespace Mews.Eet.Communication
{
    public class SendRevenueMessage
    {
        public SendRevenueMessage(RevenueRecord record, EetMode mode)
        {
            RevenueRecord = record;
            EetMode = mode;
        }

        private RevenueRecord RevenueRecord { get; }

        private EetMode EetMode { get; }

        public SendRevenueXmlMessage GetXmlMessage()
        {
            return new SendRevenueXmlMessage
            {
                Header = GetRevenueHeader(),
                Data = GetRevenueData()
            };
        }

        private RevenueHeader GetRevenueHeader()
        {
            return new RevenueHeader
            {
                MessageUuid = RevenueRecord.Identifier.ToString(),
                Sent = DateTimeConverter.ToEetDateTime(DateTimeProvider.Now),
                FirstTry = RevenueRecord.IsFirstAttempt,
                Verification = EetMode == EetMode.Verification,
                VerificationSpecified = EetMode == EetMode.Verification
            };
        }

        private RevenueData GetRevenueData()
        {
            var revenue = RevenueRecord.Revenue;
            return new RevenueData
            {
                TaxPayerTaxIdentifier = RevenueRecord.Identification.TaxPayerIdentifier.Value,
                MandantingTaxPayerIdentifier = RevenueRecord.Identification.MandantingTaxPayerIdentifier?.Value,
                RegistryIdentifier = RevenueRecord.Identification.RegistryIdentifier.Value,
                RegistrationUnitIdentifier = RevenueRecord.Identification.RegistrationUnitIdentifier.Value,

                MandatedByMultipleTaxPayers = RevenueRecord.MandatedByMultipleTaxPayers,
                MandatedByMultipleTaxPayersSpecified = RevenueRecord.MandatedByMultipleTaxPayers,

                BillNumber = RevenueRecord.BillNumber.Value,
                Accepted = DateTimeConverter.ToEetDateTime(RevenueRecord.Revenue.Accepted),
                Total = RevenueRecord.Revenue.Gross.Value,

                DepositSpecified = revenue.Deposit.IsDefined(),
                Deposit = revenue.Deposit.GetOrDefault(),

                DepositUsedSpecified = revenue.UsedDeposit.IsDefined(),
                DepositUsed = revenue.UsedDeposit.GetOrDefault()
            };
        }
    }
}
