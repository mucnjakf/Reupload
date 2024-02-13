using System.Net;
using Reupload.Server.Enums;

namespace Reupload.Server.Exceptions;

public class UserException : BaseException
{
    public UserException(ErrorCode errorCode, HttpStatusCode httpStatusCode, string message) : base(errorCode, httpStatusCode, message)
    {
    }
}