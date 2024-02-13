using System.Net;
using Reupload.Server.Enums;
using Reupload.Server.Exceptions;
using Reupload.Server.Models;
using Reupload.Shared.Constants;

namespace Reupload.Server.Dtos.UserActions;

public class UserActionTableDto
{
    public Guid Id { get; set; }

    public string UserUsername { get; set; } = default!;
    
    public string UserFullname { get; set; } = default!;

    public DateTime Timestamp { get; set; }

    public ActionType ActionType { get; set; }

    public static UserActionTableDto FromUserAction(UserAction userAction)
    {
        try
        {
            return new UserActionTableDto
            {
                Id = userAction.Id,
                UserUsername = userAction.ApplicationUser.UserName,
                UserFullname = $"{userAction.ApplicationUser.FirstName} {userAction.ApplicationUser.LastName}",
                Timestamp = userAction.Timestamp,
                ActionType = userAction.ActionType
            };
        }
        catch (Exception)
        {
            throw new MappingException(
                ErrorCode.Mapping,
                HttpStatusCode.InternalServerError,
                $"Mapping {nameof(UserAction)} to {nameof(UserActionTableDto)} failed.");
        }
    }
}