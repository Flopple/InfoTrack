using System.Net;
using InfoTrack.Domain.Responses;
using InfoTrack.Web.API.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrack.Tests.ControllerTests.UtilityTests;

public class ApiResponseFactoryTests
{
    [Fact]
    public void CreateResponse_GivenOkResponse_ReturnsOkObjectResult()
    {
        var response = new ApiResponse<string>("Success", HttpStatusCode.OK);

        var result = ApiResponseFactory.CreateResponse(response);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public void CreateResponse_GivenNotFoundResponse_ReturnsNotFoundObjectResult()
    {
        var response = new ApiResponse<string>("Not Found", HttpStatusCode.NotFound);

        var result = ApiResponseFactory.CreateResponse(response);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(response, notFoundResult.Value);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public void CreateResponse_GivenBadRequestResponse_ReturnsBadRequestObjectResult()
    {
        var response = new ApiResponse<string>("Bad Request", HttpStatusCode.BadRequest);

        var result = ApiResponseFactory.CreateResponse(response);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(response, badRequestResult.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public void CreateResponse_GivenCreatedResponse_ReturnsObjectResultWith201()
    {
        var response = new ApiResponse<string>("Created", HttpStatusCode.Created);

        var result = ApiResponseFactory.CreateResponse(response);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(response, objectResult.Value);
        Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
    }

    [Fact]
    public void CreateResponse_GivenUnhandledStatusCode_ReturnsInternalServerError()
    {
        var response = new ApiResponse<string>("Error", HttpStatusCode.InternalServerError);

        var result = ApiResponseFactory.CreateResponse(response);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(response, objectResult.Value);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }
}