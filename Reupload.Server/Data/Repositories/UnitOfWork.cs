using Reupload.Server.Data.Repositories.Contracts;

namespace Reupload.Server.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    private IPostRepository _postRepository = default!;

    private IPackageRepository _packageRepository = default!;

    private IHashtagRepository _hashtagRepository = default!;

    private IUserActionRepository _userActionRepository = default!;
    
    private IPackageConsumptionRepository _packageConsumptionRepository = default!;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IPostRepository Posts
    {
        get { return _postRepository ??= new PostRepository(_dbContext); }
    }

    public IPackageRepository Packages
    {
        get { return _packageRepository ??= new PackageRepository(_dbContext); }
    }

    public IHashtagRepository Hashtags
    {
        get { return _hashtagRepository ??= new HashtagRepository(_dbContext); }
    }
    
    public IUserActionRepository UserActions
    {
        get { return _userActionRepository ??= new UserActionRepository(_dbContext); }
    }
    
    public IPackageConsumptionRepository PackageConsumptions
    {
        get { return _packageConsumptionRepository ??= new PackageConsumptionRepository(_dbContext); }
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}