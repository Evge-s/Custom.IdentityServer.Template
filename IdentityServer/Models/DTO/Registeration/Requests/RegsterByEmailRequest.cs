using IdentityServer.Models.CustomValidation;
using IdentityServer.Models.CustomValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.DTO.Registeration.Requests
{
    public class RegsterByEmailRequest : PassValidator
    {
        [Required(ErrorMessage = "The email field is required")]
        [EmailPatternValidation]
        public string Email { get; set; }
    }
}
