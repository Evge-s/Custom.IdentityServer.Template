using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IFTurist.Models.UserModels
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }
    }
}
