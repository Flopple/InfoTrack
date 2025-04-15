using System.Net;
using InfoTrack.Domain.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrack.Web.API.Utilities;

public static class ApiResponseFactory
{
    public static IActionResult CreateResponse<T>(ApiResponse<T> data)
    {
        return data.StatusCode switch
        {
            HttpStatusCode.OK => CreateSuccessResponse(data),
            HttpStatusCode.NotFound => CreateNotFoundResponse(data),
            HttpStatusCode.BadRequest => CreateBadRequestResponse(data),
            HttpStatusCode.Created => CreateCreatedResponse(data),
            _ => CreateErrorResponse(data)
        };
    }

    public static OkObjectResult CreateSuccessResponse<T>(T data) => new OkObjectResult(data);

    public static NotFoundObjectResult CreateNotFoundResponse<T>(T data) => new NotFoundObjectResult(data);

    public static BadRequestObjectResult CreateBadRequestResponse<T>(T data) => new BadRequestObjectResult(data);

    public static ObjectResult CreateCreatedResponse<T>(T data) => new ObjectResult(data)
    {
        StatusCode = StatusCodes.Status201Created
    };

    public static ObjectResult CreateErrorResponse<T>(T data) => new ObjectResult(data)
    {
        StatusCode = StatusCodes.Status500InternalServerError
    };
}