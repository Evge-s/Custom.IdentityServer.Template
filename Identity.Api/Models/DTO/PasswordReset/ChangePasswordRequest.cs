using System.ComponentModel.DataAnnotations;
using Identity.Api.Models.CustomValidation;
using Identity.Api.Models.CustomValidation.Attributes;

namespace Identity.Api.Models.DTO.PasswordReset;

public class ChangePasswordRequest : PassValidator
{
    [Required(ErrorMessage = "The password field is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must contain at least 6 characters")]
    [PasswordCharacter]
    [PasswordDigit]
    public string NewPassword { get; set; }
    
    [Compare("NewPassword", ErrorMessage = "Confirm password doesn't match")]
    public string ConfirmNewPassword { get; set; }
}