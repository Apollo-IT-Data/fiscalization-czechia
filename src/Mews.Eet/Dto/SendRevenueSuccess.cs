namespace Mews.Eet.Dto
{
    public class SendRevenueSuccess
    {
        public SendRevenueSuccess(string confirmationCode)
        {
            ConfirmationCode = confirmationCode;
        }

        public string ConfirmationCode { get; }
    }
}
