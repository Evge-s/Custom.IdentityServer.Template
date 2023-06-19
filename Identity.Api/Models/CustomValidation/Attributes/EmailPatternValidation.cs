using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Identity.Models.CustomValidation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailPatternValidation : ValidationAttribute
    {
        public EmailPatternValidation()
        {
            ErrorMessage = "Invalid email address format";
        }

        public override bool IsValid(object value)
        {
            if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
                return false;

            string email = value.ToString();

            string emailPattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            return Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase);
        }
    }
}
