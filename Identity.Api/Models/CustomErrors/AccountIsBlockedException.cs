namespace Identity.Api.Models.CustomErrors;

public class AccountIsBlockedException : Exception
{
    public AccountIsBlockedException(string message) : base(message)
    {
    }
}