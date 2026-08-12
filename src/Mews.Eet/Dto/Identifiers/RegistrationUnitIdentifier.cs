namespace Mews.Eet.Dto.Identifiers
{
    public class RegistrationUnitIdentifier : IntIdentifier
    {
        public RegistrationUnitIdentifier(int value)
            : base(value, 1, 999999)
        {
        }
    }
}
