using Identity.Api.Models.ServiceData.Tokens;
using Identity.Api.Models.ServiceData.UserData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Api.Models.ServiceData.BuilderModels;

public class ConfirmationEmailEntityTypeConfiguration : IEntityTypeConfiguration<ConfirmationEmailCode>
{
    public void Configure(EntityTypeBuilder<ConfirmationEmailCode> builder)
    {
        builder.ToTable(nameof(ConfirmationEmailCode));

        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Email)
            .HasMaxLength(75)
            .IsRequired(true);
        
        builder.Property(x => x.Code)
            .IsRequired(true);
        
        builder.Property(x => x.Created)
            .IsRequired(true);
    }
}