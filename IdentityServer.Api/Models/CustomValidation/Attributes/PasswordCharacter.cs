using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Api.Models.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PasswordCharacter : ValidationAttribute
    {
        public PasswordCharacter()
        {
            ErrorMessage = "The password must contain at least one special character";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                string password = value.ToString();

                if (!password.Any(c => !char.IsLetterOrDigit(c)))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
