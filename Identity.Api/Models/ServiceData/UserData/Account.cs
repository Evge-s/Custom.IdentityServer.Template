using System.ComponentModel.DataAnnotations.Schema;
using Identity.Api.Models.ServiceData.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Identity.Api.Models.ServiceData.UserData
{
    public class Account
    {
        public Guid Id { get; set; }

        public string Login { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
        
        [Column(TypeName = "nvarchar(20)")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Role Role { get; set; } = Role.Customer;

        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = default;
    }
}
