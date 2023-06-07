using IdentityServer.Api.Models.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Api.Models.CustomValidation
{
    public abstract class PassValidator
    {
        [Required(ErrorMessage = "The password field is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must contain at least 6 characters")]
        [PasswordCharacter]
        [PasswordDigit]
        public string Password { get; set; }
    }
}
