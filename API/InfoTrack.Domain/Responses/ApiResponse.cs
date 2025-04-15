using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace InfoTrack.Domain.Responses;

[ExcludeFromCodeCoverage]
public record ApiResponse<T>
{
    public T Data { get; init; }
    public string Message { get; init; }
    public HttpStatusCode StatusCode { get; init; }

    public ApiResponse(
        T data,
        HttpStatusCode statusCode,
        string message = null
    )
    {
        Data = data;
        StatusCode = statusCode;
        Message = message;
    }
}