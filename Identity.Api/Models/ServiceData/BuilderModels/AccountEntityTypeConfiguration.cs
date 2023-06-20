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

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Login)
                .HasMaxLength(75)
                .IsRequired(true);

            builder.Property(x => x.PasswordHash)
                .IsRequired(true);

            builder.Property(x => x.PasswordSalt)
                .IsRequired(true);

            builder.Property(x => x.Role)
                .HasConversion(
                v => v.Id,
                v => Enumeration.FromValue<Role>(v));

            builder.HasMany(x => x.RefreshTokens)
                .WithOne(t => t.Account)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();
        }
    }
}
