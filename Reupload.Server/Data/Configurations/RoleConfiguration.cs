using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reupload.Shared.Constants;

namespace Reupload.Server.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole { Id = "2e6ca4da-8fef-4167-bfd3-4ea02e5d2062", Name = Roles.Admin, NormalizedName = Roles.Admin.ToUpper() },
            new IdentityRole { Id = "71e23399-bcad-4fb1-bede-179e3bc7410b", Name = Roles.User, NormalizedName = Roles.User.ToUpper() });
    }
}