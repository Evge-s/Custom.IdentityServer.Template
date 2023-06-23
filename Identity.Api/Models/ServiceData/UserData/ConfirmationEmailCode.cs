namespace Identity.Api.Models.ServiceData.UserData;

public class ConfirmationEmailCode
{
    public ConfirmationEmailCode(string code, string email)
    {
        Email = email;
        Code = code;
    }

    public string Email { get; set; }
    
    public string Code { get; set; }

    public bool Confirmed { get; set; }
    
    public  DateTime Created { get; set; } = DateTime.UtcNow;
    
    public bool IsExpired => DateTime.UtcNow > Created.AddMinutes(30);
    
    public bool IsActive => !IsExpired;
}