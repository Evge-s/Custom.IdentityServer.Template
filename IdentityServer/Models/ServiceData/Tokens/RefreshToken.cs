using IdentityServer.Models.Data.UserData;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.Data.Tokens
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
