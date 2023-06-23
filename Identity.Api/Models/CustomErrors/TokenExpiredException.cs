namespace Identity.Api.Models.CustomErrors;

public class TokenExpiredException: Exception
{
    public TokenExpiredException(string message) : base(message)
    {
    }
}