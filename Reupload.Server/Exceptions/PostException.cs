using System.Net;
using Reupload.Server.Enums;

namespace Reupload.Server.Exceptions;

public class PostException : BaseException
{
    public PostException(ErrorCode errorCode, HttpStatusCode httpStatusCode, string message) : base(errorCode, httpStatusCode, message)
    {
    }
}