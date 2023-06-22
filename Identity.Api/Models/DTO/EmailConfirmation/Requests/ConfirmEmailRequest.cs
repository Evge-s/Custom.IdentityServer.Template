using System.ComponentModel.DataAnnotations;
using Identity.Api.Models.CustomValidation.Attributes;

namespace Identity.Api.Models.DTO.EmailConfirmation.Requests
{
    public class ConfirmEmailRequest
    {
        [Required(ErrorMessage = "The email is required")]
        [EmailPatternValidation]
        public string Email { get; init; }
        
        [Required(ErrorMessage = "The Confirmation Code is required")]
        public string ConfirmationCode { get; init; }
    }
}
