using System.Net;
using System.Text.Json.Serialization;
using Reupload.Server.Enums;

namespace Reupload.Server.Dtos;

public class ErrorResponseDto
{
    public ErrorCode ErrorCode { get; set; }

    public List<string> Errors { get; set; } = default!;

    [JsonIgnore]
    public HttpStatusCode HttpStatusCode { get; set; }
}