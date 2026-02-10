using Amazon.CognitoIdentityProvider.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Security.Claims;
using VideoCore.Auth.DTO;
using VideoCore.Auth.Services;

public class VideoCoreAuthTests
{
    private readonly Mock<ILogger<VideoCore.Auth.VideoCoreAuth>> _loggerMock;
    private readonly Mock<ICognitoService> _cognitoServiceMock;
    private readonly VideoCore.Auth.VideoCoreAuth _function;

    public VideoCoreAuthTests()
    {
        _loggerMock = new Mock<ILogger<VideoCore.Auth.VideoCoreAuth>>();
        _cognitoServiceMock = new Mock<ICognitoService>();
        _function = new VideoCore.Auth.VideoCoreAuth(_loggerMock.Object, _cognitoServiceMock.Object);
    }

    #region ValidateToken Tests

    [Fact]
    public async Task ValidateToken_WithValidToken_ShouldReturnOkResult()
    {
        // Arrange
        var query = new Dictionary<string, string>
        {
            { "access_token", "valid-token" },
            { "url", "/videos" },
            { "http_method", "GET" }
        };

        var httpRequest = CreateMockHttpRequestWithQuery(query);

        var claims = new List<Claim>
        {
            new("sub", "user-sub-123"),
            new("email", "test@test.com"),
            new("custom:role", "CUSTOMER")
        };

        var jwt = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(claims: claims);

        _cognitoServiceMock
            .Setup(s => s.ValidateToken("valid-token"))
            .ReturnsAsync(jwt);

        _cognitoServiceMock
            .Setup(s => s.GetUserBySubAsync("user-sub-123"))
            .ReturnsAsync(new UserType
            {
                Username = "user-sub-123",
                Attributes =
                [
                    new() { Name = "sub", Value = "user-sub-123" },
                    new() { Name = "email", Value = "test@test.com" }
                ]
            });


        // Act
        var result = await _function.ValidateToken(httpRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)result;
        ok.Value.Should().BeOfType<UserDetailsDTO>();
    }

    [Fact]
    public async Task ValidateToken_WhenAccessTokenMissing_ShouldReturnUnauthorized()
    {
        // Arrange
        var query = new Dictionary<string, string>
        {
            { "url", "/videos" },
            { "http_method", "GET" }
        };

        var httpRequest = CreateMockHttpRequestWithQuery(query);

        // Act
        var result = await _function.ValidateToken(httpRequest);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task ValidateToken_WhenJwtHasNoSub_ShouldReturnUnauthorized()
    {
        // Arrange
        var query = new Dictionary<string, string>
        {
            { "access_token", "token" },
            { "url", "/videos" },
            { "http_method", "GET" }
        };

        var httpRequest = CreateMockHttpRequestWithQuery(query);

        var jwt = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken();

        _cognitoServiceMock
            .Setup(s => s.ValidateToken("token"))
            .ReturnsAsync(jwt);

        // Act
        var result = await _function.ValidateToken(httpRequest);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task ValidateToken_WhenUserNotFound_ShouldReturnUnauthorized()
    {
        // Arrange
        var query = new Dictionary<string, string>
        {
            { "access_token", "token" },
            { "url", "/videos" },
            { "http_method", "GET" }
        };

        var httpRequest = CreateMockHttpRequestWithQuery(query);

        var jwt = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
            claims: new[] { new Claim("sub", "missing-sub") });

        _cognitoServiceMock
            .Setup(s => s.ValidateToken("token"))
            .ReturnsAsync(jwt);

        _cognitoServiceMock
            .Setup(s => s.GetUserBySubAsync("missing-sub"))
            .ReturnsAsync((UserType?)null);

        // Act
        var result = await _function.ValidateToken(httpRequest);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task ValidateToken_WhenUnexpectedException_ShouldReturnInternalServerError()
    {
        // Arrange
        var query = new Dictionary<string, string>
        {
            { "access_token", "token" },
            { "url", "/videos" },
            { "http_method", "GET" }
        };

        var httpRequest = CreateMockHttpRequestWithQuery(query);

        _cognitoServiceMock
            .Setup(s => s.ValidateToken("token"))
            .ThrowsAsync(new Exception("boom"));

        // Act
        var result = await _function.ValidateToken(httpRequest);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var obj = (ObjectResult)result;
        obj.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    #endregion

    private static HttpRequestData CreateMockHttpRequestWithQuery(
        IDictionary<string, string> queryValues)
    {
        var context = new Mock<FunctionContext>();
        var request = new Mock<HttpRequestData>(context.Object);

        var query = new System.Collections.Specialized.NameValueCollection();
        foreach (var kv in queryValues)
            query.Add(kv.Key, kv.Value);

        request.Setup(r => r.Query).Returns(query);
        request.Setup(r => r.Url).Returns(new Uri("http://localhost/api/validate"));

        return request.Object;
    }
}