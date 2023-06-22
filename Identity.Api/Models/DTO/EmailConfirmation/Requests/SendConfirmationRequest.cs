using System.ComponentModel.DataAnnotations;
using Identity.Api.Models.CustomValidation.Attributes;

namespace Identity.Api.Models.DTO.EmailConfirmation.Requests;

public class SendConfirmationRequest
{
    [Required(ErrorMessage = "The email is required")]
    [EmailPatternValidation]
    public string Email { get; init; }
}