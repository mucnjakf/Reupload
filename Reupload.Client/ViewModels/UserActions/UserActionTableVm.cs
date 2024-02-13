using Reupload.Shared.Constants;

namespace Reupload.Client.ViewModels.UserActions;

public class UserActionTableVm
{
    public Guid Id { get; set; }

    public string UserUsername { get; set; } = default!;
    
    public string UserFullname { get; set; } = default!;

    public DateTime Timestamp { get; set; }

    public ActionType ActionType { get; set; }
}