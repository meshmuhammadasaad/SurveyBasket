using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.Api.Abstractions.Consts;

namespace SurveyBasket.Api.Persistence.EntitiesConfigurations;

public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        //Default Data
        var permissions = Permissions.GetAllPermissins();
        var adminClaims = new List<IdentityRoleClaim<string>>();
        for (int i = 0; i < permissions.Count; i++)
        {
            adminClaims.Add(new IdentityRoleClaim<string>
            {
                Id = i + 1,
                ClaimType = Permissions.Type,
                ClaimValue = permissions[i],
                RoleId = DefaultRoles.AdminRoleId
            });
        }

        builder.HasData(adminClaims);
    }
}
