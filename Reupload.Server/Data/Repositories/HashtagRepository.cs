using Reupload.Server.Data.Repositories.Contracts;
using Reupload.Server.Models;

namespace Reupload.Server.Data.Repositories;

public class HashtagRepository : Repository<ApplicationDbContext, Hashtag>, IHashtagRepository
{
    public HashtagRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}