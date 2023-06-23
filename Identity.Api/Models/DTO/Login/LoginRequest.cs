using System.ComponentModel.DataAnnotations;
using Identity.Api.Models.CustomValidation;
using Identity.Api.Models.CustomValidation.Attributes;

namespace Identity.Api.Models.DTO.Login.Request;

public class LoginRequest : PassValidator
{
    [Required(ErrorMessage = "The email field is required")]
    [EmailPatternValidation]
    public string Email { get; init; }
}