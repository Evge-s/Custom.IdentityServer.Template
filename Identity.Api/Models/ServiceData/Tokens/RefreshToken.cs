using System.ComponentModel.DataAnnotations;
using Identity.Api.Models.ServiceData.UserData;

namespace Identity.Api.Models.ServiceData.Tokens
{
    public class RefreshToken
    {
        public Account Account { get; set; }

        public Guid AccountId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime Expires { get; set; }

        [Required]
        public DateTime Created { get; set; }
        public string? CreatedByIp { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => !IsExpired;
    }
}
