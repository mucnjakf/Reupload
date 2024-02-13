using System.Net;
using Reupload.Server.Enums;

namespace Reupload.Server.Exceptions;

public class BaseException : Exception
{
    public ErrorCode ErrorCode { get; set; }

    public HttpStatusCode HttpStatusCode { get; set; }

    public BaseException(
        ErrorCode errorCode,
        HttpStatusCode httpStatusCode,
        string message)
        : base(message)
    {
        ErrorCode = errorCode;
        HttpStatusCode = httpStatusCode;
    }
}