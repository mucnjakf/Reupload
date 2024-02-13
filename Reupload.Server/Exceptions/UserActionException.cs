using System.Net;
using Reupload.Server.Enums;

namespace Reupload.Server.Exceptions;

public class UserActionException : BaseException
{
    public UserActionException(ErrorCode errorCode, HttpStatusCode httpStatusCode, string message) : base(errorCode, httpStatusCode,
        message)
    {
    }
}