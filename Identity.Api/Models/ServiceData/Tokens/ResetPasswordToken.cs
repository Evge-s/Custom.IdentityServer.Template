namespace Identity.Api.Models.ServiceData.Tokens;

public class ResetPasswordToken
{
    public ResetPasswordToken(string token, string email)
    {
        Email = email;
        ResetToken = token;
    }

    public string Email { get; set; }
    
    public string ResetToken { get; set; }

    public  DateTime Created { get; set; } = DateTime.UtcNow;
    
    public bool IsExpired => DateTime.UtcNow > Created.AddMinutes(60);
    
    public bool IsActive => !IsExpired;
}