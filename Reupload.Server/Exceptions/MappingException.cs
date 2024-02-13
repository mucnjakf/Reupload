using System.Net;
using Reupload.Server.Enums;

namespace Reupload.Server.Exceptions;

public class MappingException : BaseException
{
    public MappingException(
        ErrorCode errorCode,
        HttpStatusCode httpStatusCode,
        string message) : base(errorCode, httpStatusCode, message)
    {
    }
}