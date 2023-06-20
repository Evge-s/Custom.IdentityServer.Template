namespace Identity.Api.Models.ServiceData.UserData;

public class ConfirmationEmailCode
{
    public Guid Id { get; set; }
    
    public string Email { get; set; }
    
    public int Code { get; set; }
    
    public  DateTime Created { get; set; } = DateTime.UtcNow;
    
    public bool IsExpired => DateTime.UtcNow > Created.AddMinutes(30);
    
    public bool IsActive => !IsExpired;
}