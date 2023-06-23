using Identity.Api.Models.ServiceData.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Api.Models.ServiceData.BuilderModels;

public class ResetPasswordTokenEntityTypeConfiguration: IEntityTypeConfiguration<ResetPasswordToken>
{
    public void Configure(EntityTypeBuilder<ResetPasswordToken> builder)
    {
        builder.ToTable(nameof(ResetPasswordToken));

        builder.Property(x => x.Email)
            .HasMaxLength(75)
            .IsRequired(true);
        
        builder.Property(x => x.ResetToken)
            .IsRequired(true);
        
        builder.Property(x => x.Created)
            .IsRequired(true);
    }
}