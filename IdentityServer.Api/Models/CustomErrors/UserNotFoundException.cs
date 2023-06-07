namespace IdentityServer.Api.Models.CustomErrors
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message)
        {
        }
    }
}
