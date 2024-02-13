namespace Reupload.Server.Dtos.UserActions;

public class UpdatePackageUserActionDto
{
    public Guid OldPackageId { get; set; }
    
    public Guid NewPackageId { get; set; }
}