using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reupload.Server.Models;

namespace Reupload.Server.Data.Configurations;

public class UserActionConfiguration : IEntityTypeConfiguration<UserAction>
{
    public void Configure(EntityTypeBuilder<UserAction> builder)
    {
        builder.ToTable("UserActions").HasKey(x => x.Id);
    }
}