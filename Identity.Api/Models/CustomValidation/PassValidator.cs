using Identity.Models.CustomValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Identity.Models.CustomValidation
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
