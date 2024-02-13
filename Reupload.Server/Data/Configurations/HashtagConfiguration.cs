using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reupload.Server.Models;

namespace Reupload.Server.Data.Configurations;

public class HashtagConfiguration : IEntityTypeConfiguration<Hashtag>
{
    public void Configure(EntityTypeBuilder<Hashtag> builder)
    {
        builder
            .ToTable("Hashtags")
            .HasKey(hashtag => hashtag.Id);
    }
}