namespace Mews.Eet.Dto
{
    public class Revenue
    {
        public Revenue(CurrencyValue gross, DateTimeWithTimeZone accepted = null, CurrencyValue deposit = null, CurrencyValue usedDeposit = null)
        {
            Accepted = accepted ?? DateTimeProvider.Now;
            Gross = gross;
            Deposit = deposit;
            UsedDeposit = usedDeposit;
        }

        public DateTimeWithTimeZone Accepted { get; }

        public CurrencyValue Gross { get; }

        public CurrencyValue Deposit { get; }

        public CurrencyValue UsedDeposit { get; }
    }
}
