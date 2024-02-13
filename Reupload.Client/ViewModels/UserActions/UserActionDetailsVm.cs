using Reupload.Shared.Constants;

namespace Reupload.Client.ViewModels.UserActions;

public class UserActionDetailsVm
{
    public Guid Id { get; set; }

    public UserActionUserVm User { get; set; } = default!;

    public DateTime Timestamp { get; set; }

    public ActionType ActionType { get; set; }

    public string ActionParametersJson { get; set; } = default!;
}