using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IFTurist.Models.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "No email is assigned!")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Incorrect email")]
        public string Email { get; set; }

        [StringLength(12, MinimumLength = 6, ErrorMessage = "Password length must be 6 or more characters")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords don't match!")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
