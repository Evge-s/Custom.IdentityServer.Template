using Identity.Api.Models.ServiceData.Tokens;

namespace Identity.Api.Models.ServiceData.UserData
{
    public class Account
    {
        public Guid Id { get; set; }

        public string Login { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        // set role "Customer" by default
        public Role Role { get; set; } = Role.From(2);

        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = default;
    }
}
