using Identity.Api.Models.ServiceData.UserData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Api.Models.ServiceData.BuilderModels
{
    public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable(nameof(Account));

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Email)
                .HasMaxLength(75)
                .IsRequired(true);

            builder.Property(a => a.PasswordHash)
                .IsRequired(true);

            builder.Property(a => a.PasswordSalt)
                .IsRequired(true);

            builder.Property(a => a.Role)
                .HasConversion<string>();

            builder.Property(a => a.Blocked);

            builder.HasMany(a => a.RefreshTokens)
                .WithOne(t => t.Account)
                .HasPrincipalKey(t => t.Id)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.UpdatedAt)
                .IsRequired();
        }
    }
}
