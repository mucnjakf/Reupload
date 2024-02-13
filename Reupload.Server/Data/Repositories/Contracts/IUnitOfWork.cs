namespace Reupload.Server.Data.Repositories.Contracts;

public interface IUnitOfWork
{
    public IPostRepository Posts { get; }

    public IPackageRepository Packages { get; }

    public IHashtagRepository Hashtags { get; }

    public IUserActionRepository UserActions { get; }

    public IPackageConsumptionRepository PackageConsumptions { get; }

    Task SaveChangesAsync();
}