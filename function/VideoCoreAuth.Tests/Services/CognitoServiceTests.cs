using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using FluentAssertions;
using Moq;
using VideoCore.Auth.Model;
using VideoCore.Auth.Services;

namespace VideoCoreAuth.Tests.Services;

public class CognitoServiceTests
{
    private readonly Mock<IAmazonCognitoIdentityProvider> _cognitoMock;
    private readonly CognitoSettings _settings;
    private readonly CognitoService _service;

    public CognitoServiceTests()
    {
        _cognitoMock = new Mock<IAmazonCognitoIdentityProvider>();
        _settings = new CognitoSettings
        {
            UserPoolId = "us-east-1_test",
            AppClientId = "test-client-id",
            Region = "us-east-1"
        };

        _service = new CognitoService(_cognitoMock.Object, _settings);
    }

    #region GetUserBySubAsync Tests

    [Fact]
    public async Task GetUserBySubAsync_WhenUserFound_ShouldReturnUser()
    {
        // Arrange
        var expectedUser = new UserType
        {
            Username = "subuser",
            Attributes = new List<AttributeType>
            {
                new() { Name = "sub", Value = "abc-123" }
            }
        };

        _cognitoMock
            .Setup(c => c.ListUsersAsync(
                It.Is<ListUsersRequest>(r =>
                    r.UserPoolId == _settings.UserPoolId &&
                    r.Filter == "sub = \"abc-123\""),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse
            {
                Users = new List<UserType> { expectedUser }
            });

        // Act
        var result = await _service.GetUserBySubAsync("abc-123");

        // Assert
        result.Should().NotBeNull();
        result!.Username.Should().Be("subuser");
    }

    [Fact]
    public async Task GetUserBySubAsync_WhenNoUserFound_ShouldReturnNull()
    {
        // Arrange
        _cognitoMock
            .Setup(c => c.ListUsersAsync(
                It.IsAny<ListUsersRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListUsersResponse
            {
                Users = new List<UserType>()
            });

        // Act
        var result = await _service.GetUserBySubAsync("nonexistent-sub");

        // Assert
        result.Should().BeNull();
    }

    #endregion
}