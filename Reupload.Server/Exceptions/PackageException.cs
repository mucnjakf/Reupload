using System.Net;
using Reupload.Server.Enums;

namespace Reupload.Server.Exceptions;

public class PackageException : BaseException
{
    public PackageException(ErrorCode errorCode, HttpStatusCode httpStatusCode, string message) : base(errorCode, httpStatusCode, message)
    {
    }
}