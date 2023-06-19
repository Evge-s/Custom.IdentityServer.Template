using Identity.Models.CustomValidation.Attributes;
using Identity.Models.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace Identity.Models.DTO.Registeration.Requests
{
    public class RegsterByEmailRequest : PassValidator
    {
        [Required(ErrorMessage = "The email field is required")]
        [EmailPatternValidation]
        public string Email { get; set; }
    }
}
