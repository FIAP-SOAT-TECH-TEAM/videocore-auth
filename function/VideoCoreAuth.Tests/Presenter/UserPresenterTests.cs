using System.Security.Claims;
using Amazon.CognitoIdentityProvider.Model;
using FluentAssertions;
using VideoCore.Auth.DTO;
using VideoCore.Auth.Presenter;

namespace VideoCoreAuth.Tests.Presenter;

public class UserPresenterTests
{
    #region ToUserDetailsDTO Tests

    [Fact]
    public void ToUserDetailsDTO_WithUserAndClaims_ShouldReturnCompleteDTO()
    {
        // Arrange
        var user = new UserType
        {
            Attributes = new List<AttributeType>
            {
                new AttributeType { Name = "name", Value = "João Silva" },
                new AttributeType { Name = "email", Value = "joao@example.com" }
            },
            UserCreateDate = new DateTime(2024, 1, 15, 10, 30, 0)
        };

        var claims = new List<Claim>
        {
            new Claim("sub", "user-sub-123")
        };

        // Act
        var result = UserPresenter.ToUserDetailsDTO(user, claims);

        // Assert
        result.Should().NotBeNull();
        result.Subject.Should().Be("user-sub-123");
        result.Name.Should().Be("João Silva");
        result.Email.Should().Be("joao@example.com");
    }

    [Fact]
    public void ToUserDetailsDTO_WithNullUser_ShouldReturnDTOWithDefaults()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim("sub", "user-sub-456")
        };

        // Act
        var result = UserPresenter.ToUserDetailsDTO(null, claims);

        // Assert
        result.Should().NotBeNull();
        result.Subject.Should().Be("user-sub-456");
        result.Name.Should().BeEmpty();
        result.Email.Should().BeEmpty();
    }

    [Fact]
    public void ToUserDetailsDTO_WithNullClaims_ShouldReturnDTOWithEmptySubject()
    {
        // Arrange
        var user = new UserType
        {
            Attributes = new List<AttributeType>
            {
                new AttributeType { Name = "name", Value = "Maria" },
                new AttributeType { Name = "email", Value = "maria@test.com" }
            },
            UserCreateDate = new DateTime(2024, 2, 20)
        };

        // Act
        var result = UserPresenter.ToUserDetailsDTO(user, null);

        // Assert
        result.Should().NotBeNull();
        result.Subject.Should().BeEmpty();
        result.Name.Should().Be("Maria");
        result.Email.Should().Be("maria@test.com");
    }

    [Fact]
    public void ToUserDetailsDTO_WithBothNull_ShouldReturnDTOWithAllDefaults()
    {
        // Act
        var result = UserPresenter.ToUserDetailsDTO(null, null);

        // Assert
        result.Should().NotBeNull();
        result.Subject.Should().BeEmpty();
        result.Name.Should().BeEmpty();
        result.Email.Should().BeEmpty();
    }

    [Fact]
    public void ToUserDetailsDTO_WithEmptyClaims_ShouldReturnDTOWithEmptySubject()
    {
        // Arrange
        var claims = new List<Claim>();

        // Act
        var result = UserPresenter.ToUserDetailsDTO(null, claims);

        // Assert
        result.Should().NotBeNull();
        result.Subject.Should().BeEmpty();
    }

    [Fact]
    public void ToUserDetailsDTO_WithPartialUserAttributes_ShouldReturnDTOWithAvailableData()
    {
        // Arrange
        var user = new UserType
        {
            Attributes = new List<AttributeType>
            {
                new AttributeType { Name = "name", Value = "Test User" }
            }
        };

        // Act
        var result = UserPresenter.ToUserDetailsDTO(user, null);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test User");
        result.Email.Should().BeEmpty();
    }

    [Fact]
    public void ToUserDetailsDTO_ShouldReturnUserDetailsDTOType()
    {
        // Act
        var result = UserPresenter.ToUserDetailsDTO(null, null);

        // Assert
        result.Should().BeOfType<UserDetailsDto>();
    }

    #endregion
}
