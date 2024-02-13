using Reupload.Server.Enums;
using Reupload.Shared.Constants;

namespace Reupload.Server.Dtos.UserActions;

public class UserActionInsertDto
{
    public string UserId { get; set; } = default!;

    public DateTime Timestamp { get; set; }

    public ActionType ActionType { get; set; }

    public string ActionParametersJson { get; set; } = default!;
}