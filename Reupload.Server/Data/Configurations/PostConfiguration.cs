using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reupload.Server.Models;

namespace Reupload.Server.Data.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder
            .ToTable("Posts")
            .HasKey(post => post.Id);

        builder
            .HasMany(post => post.Hashtags)
            .WithMany(hashtag => hashtag.Posts)
            .UsingEntity<Dictionary<string, object>>("PostHashtag",
                j => j.HasOne<Hashtag>()
                    .WithMany()
                    .HasForeignKey("HashtagId")
                    .HasConstraintName("FK_PostHashtag_Hashtags_HashtagId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Post>()
                    .WithMany()
                    .HasForeignKey("PostId")
                    .HasConstraintName("FK_PostHashtag_Posts_PostId")
                    .OnDelete(DeleteBehavior.ClientCascade)
            );
    }
}