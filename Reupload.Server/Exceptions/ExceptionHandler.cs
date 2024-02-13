using System.Net;
using Reupload.Server.Dtos;
using Reupload.Server.Enums;

namespace Reupload.Server.Exceptions;

public class ExceptionHandler
{
    public static ErrorResponseDto Handle(Exception ex)
    {
        ErrorResponseDto errorResponseDto = new()
        {
            ErrorCode = ErrorCode.General,
            HttpStatusCode = HttpStatusCode.InternalServerError,
            Errors = new List<string> { "Unexpected error occured." }
        };

        switch (ex)
        {
            case MappingException mappingEx:
                errorResponseDto.ErrorCode = mappingEx.ErrorCode;
                errorResponseDto.HttpStatusCode = mappingEx.HttpStatusCode;
                errorResponseDto.Errors = new List<string> { mappingEx.Message };
                break;
            case PackageException packageEx:
                errorResponseDto.ErrorCode = packageEx.ErrorCode;
                errorResponseDto.HttpStatusCode = packageEx.HttpStatusCode;
                errorResponseDto.Errors = new List<string> { packageEx.Message };
                break;
            case PostException postEx:
                errorResponseDto.ErrorCode = postEx.ErrorCode;
                errorResponseDto.HttpStatusCode = postEx.HttpStatusCode;
                errorResponseDto.Errors = new List<string> { postEx.Message };
                break;
            case UserException userEx:
                errorResponseDto.ErrorCode = userEx.ErrorCode;
                errorResponseDto.HttpStatusCode = userEx.HttpStatusCode;
                errorResponseDto.Errors = new List<string> { userEx.Message };
                break;
            case UserActionException userActionEx:
                errorResponseDto.ErrorCode = userActionEx.ErrorCode;
                errorResponseDto.HttpStatusCode = userActionEx.HttpStatusCode;
                errorResponseDto.Errors = new List<string> { userActionEx.Message };
                break;
        }

        return errorResponseDto;
    }
}