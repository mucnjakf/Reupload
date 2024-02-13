using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reupload.Server.Models;

namespace Reupload.Server.Data.Configurations;

public class PackageConfiguration : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder
            .ToTable("Packages")
            .HasKey(package => package.Id);

        builder
            .HasMany(package => package.ApplicationUsers)
            .WithOne(user => user.Package)
            .HasForeignKey(user => user.PackageId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(package => package.PackageConsumptions)
            .WithOne(packageConsumption => packageConsumption.Package);

        builder.HasData(new List<Package>
        {
            new()
            {
                Id = Guid.Parse("85894f3e-9152-41e6-ba73-d3281a42ddc0"),
                Name = "Free",
                Price = 0,
                PhotoUploadLimit = 10
            },
            new()
            {
                Id = Guid.Parse("9374f8f2-1c80-481f-aba9-d7c3a352abd1"),
                Name = "Professional",
                Price = 49.99,
                PhotoUploadLimit = 100
            },
            new()
            {
                Id = Guid.Parse("CF47D685-F74A-428A-A5F9-583085973958"),
                Name = "Gold",
                Price = 99.99,
                PhotoUploadLimit = -1
            }
        });
    }
}