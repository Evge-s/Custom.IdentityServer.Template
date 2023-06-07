using IdentityServer.Api.Models.CustomValidation;
using IdentityServer.Api.Models.CustomValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Api.Models.DTO.Registeration.Requests
{
    public class RegsterByEmailRequest : PassValidator
    {
        [Required(ErrorMessage = "The email field is required")]
        [EmailPatternValidation]
        public string Email { get; set; }
    }
}
