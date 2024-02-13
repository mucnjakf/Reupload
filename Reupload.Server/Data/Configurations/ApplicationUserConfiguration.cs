using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reupload.Server.Models;

namespace Reupload.Server.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .HasMany(user => user.Posts)
            .WithOne(post => post.ApplicationUser)
            .HasForeignKey(post => post.ApplicationUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(user => user.UserActions)
            .WithOne(userAction => userAction.ApplicationUser)
            .HasForeignKey(userAction => userAction.ApplicationUserId);

        builder
            .HasOne(x => x.PackageConsumption)
            .WithOne(x => x.User)
            .HasForeignKey<PackageConsumption>(x => x.UserId);
    }
}