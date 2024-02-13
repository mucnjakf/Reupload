using System.Net;
using Reupload.Server.Enums;
using Reupload.Server.Exceptions;
using Reupload.Server.Models;
using Reupload.Shared.Constants;

namespace Reupload.Server.Dtos.UserActions;

public class UserActionDetailsDto
{
    public Guid Id { get; set; }

    public UserActionUser User { get; set; } = default!;

    public DateTime Timestamp { get; set; }

    public ActionType ActionType { get; set; }

    public string ActionParametersJson { get; set; } = default!;

    public static UserActionDetailsDto FromUserAction(UserAction userAction)
    {
        try
        {
            return new UserActionDetailsDto
            {
                Id = userAction.Id,
                User = UserActionUser.FromUser(userAction.ApplicationUser),
                Timestamp = userAction.Timestamp,
                ActionType = userAction.ActionType,
                ActionParametersJson = userAction.ActionParametersJson
            };
        }
        catch (Exception)
        {
            throw new MappingException(
                ErrorCode.Mapping,
                HttpStatusCode.InternalServerError,
                $"Mapping {nameof(UserAction)} to {nameof(UserActionDetailsDto)} failed.");
        }
    }
}