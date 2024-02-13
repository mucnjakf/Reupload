using System.Net;
using Reupload.Server.Dtos.UserActions;
using Reupload.Server.Enums;
using Reupload.Server.Exceptions;
using Reupload.Shared.Constants;

namespace Reupload.Server.Models;

public class UserAction
{
    public Guid Id { get; set; }

    public string ApplicationUserId { get; set; } = default!;

    public ApplicationUser ApplicationUser { get; set; } = default!;

    public DateTime Timestamp { get; set; }

    public ActionType ActionType { get; set; }

    public string ActionParametersJson { get; set; } = default!;

    public static UserAction FromUserActionInsertDto(UserActionInsertDto userActionInsertDto)
    {
        try
        {
            return new UserAction
            {
                ApplicationUserId = userActionInsertDto.UserId,
                Timestamp = userActionInsertDto.Timestamp,
                ActionType = userActionInsertDto.ActionType,
                ActionParametersJson = userActionInsertDto.ActionParametersJson
            };
        }
        catch (Exception)
        {
            throw new MappingException(
                ErrorCode.Mapping,
                HttpStatusCode.InternalServerError,
                $"Mapping {nameof(UserActionInsertDto)} to {nameof(UserAction)} failed.");
        }
    }
}