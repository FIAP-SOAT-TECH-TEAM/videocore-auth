using FluentAssertions;
using VideoCore.Auth.DTO;

namespace VideoCoreAuth.Tests.DTO;

public class ErrorDTOTests
{
    [Fact]
    public void ErrorDTO_ShouldSetTimestampAutomatically()
    {
        // Arrange
        var before = DateTime.UtcNow;

        // Act
        var dto = new ErrorDto
        {
            Status = 400,
            Message = "Error",
            Path = "/api/test"
        };

        var after = DateTime.UtcNow;

        // Assert
        dto.Timestamp.Should().BeOnOrAfter(before.AddSeconds(-1));
        dto.Timestamp.Should().BeOnOrBefore(after.AddSeconds(1));
    }

    [Fact]
    public void ErrorDTO_ShouldSetStatusCorrectly()
    {
        // Arrange & Act
        var dto = new ErrorDto
        {
            Status = 404,
            Message = "Not found",
            Path = "/api/users/123"
        };

        // Assert
        dto.Status.Should().Be(404);
    }

    [Fact]
    public void ErrorDTO_ShouldSetMessageCorrectly()
    {
        // Arrange & Act
        var dto = new ErrorDto
        {
            Status = 500,
            Message = "Internal server error",
            Path = "/api/test"
        };

        // Assert
        dto.Message.Should().Be("Internal server error");
    }

    [Fact]
    public void ErrorDTO_ShouldSetPathCorrectly()
    {
        // Arrange & Act
        var dto = new ErrorDto
        {
            Status = 400,
            Message = "Error",
            Path = "/api/users/create"
        };

        // Assert
        dto.Path.Should().Be("/api/users/create");
    }
}

public class UserDetailsDTOTests
{
    [Fact]
    public void UserDetailsDTO_ShouldSetSubjectCorrectly()
    {
        // Arrange & Act
        var dto = new UserDetailsDto
        {
            Subject = "sub-123",
            Name = "Test",
            Email = "test@test.com"
        };

        // Assert
        dto.Subject.Should().Be("sub-123");
    }

    [Fact]
    public void UserDetailsDTO_ShouldSetNameCorrectly()
    {
        // Arrange & Act
        var dto = new UserDetailsDto
        {
            Subject = "sub-123",
            Name = "João Silva",
            Email = "test@test.com"
        };

        // Assert
        dto.Name.Should().Be("João Silva");
    }

    [Fact]
    public void UserDetailsDTO_ShouldSetEmailCorrectly()
    {
        // Arrange & Act
        var dto = new UserDetailsDto
        {
            Subject = "sub-123",
            Name = "Test",
            Email = "joao@example.com"
        };

        // Assert
        dto.Email.Should().Be("joao@example.com");
    }

}
