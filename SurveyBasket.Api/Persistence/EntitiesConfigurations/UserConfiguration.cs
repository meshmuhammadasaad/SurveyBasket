using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.Api.Abstractions.Consts;
using SurveyBasket.Api.Entities;

namespace SurveyBasket.Api.Persistence.EntitiesConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .OwnsMany(u => u.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");

        builder.Property(u => u.FirstName).HasMaxLength(100);
        builder.Property(u => u.LastName).HasMaxLength(100);

        //Default Data
        builder.HasData(new ApplicationUser
        {
            Id = DefaultUsers.AdminId,
            FirstName = "Survey Basket",
            LastName = "Admin",
            UserName = DefaultUsers.AdminEmail,
            NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
            Email = DefaultUsers.AdminEmail,
            EmailConfirmed = true,
            NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
            ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
            SecurityStamp = DefaultUsers.AdminSecurityStamp,
            PasswordHash = DefaultUsers.AdminPasswordHash,
        });
    }
}
