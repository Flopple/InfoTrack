using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace InfoTrack.Tests.ServiceTests.Helpers;

[ExcludeFromCodeCoverage]
public class HttpMessageHandlerMock : HttpMessageHandler
{
    private string _response;
    private HttpStatusCode _statusCode;

    public HttpMessageHandlerMock(string response = "", HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        _response = response;
        _statusCode = statusCode;
    }

    public HttpMessageHandlerMock WithResponse(string response)
    {
        _response = response;
        return this;
    }

    public HttpMessageHandlerMock WithStatusCode(HttpStatusCode statusCode)
    {
        _statusCode = statusCode;
        return this;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        return new HttpResponseMessage
        {
            StatusCode = _statusCode,
            Content = new StringContent(_response)
        };
    }

    public HttpClient ToHttpClient()
    {
        return new HttpClient(this);
    }
}