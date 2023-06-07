using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Api.Models.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PasswordDigit : ValidationAttribute
    {
        public PasswordDigit()
        {
            ErrorMessage = "The password must contain at least one number";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                string password = value.ToString();

                if (!password.Any(char.IsDigit))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
