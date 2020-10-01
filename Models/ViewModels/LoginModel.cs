using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IFTurist.Models.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Invalid email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Invalid password!")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
