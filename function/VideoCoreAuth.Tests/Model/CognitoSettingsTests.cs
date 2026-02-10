using FluentAssertions;
using VideoCore.Auth.Model;

namespace VideoCoreAuth.Tests.Model;

public class CognitoSettingsTests
{
    [Fact]
    public void CognitoSettings_ShouldSetUserPoolId()
    {
        // Arrange & Act
        var settings = CreateSettings();

        // Assert
        settings.UserPoolId.Should().Be("us-east-1_testpool");
    }

    [Fact]
    public void CognitoSettings_ShouldSetAppClientId()
    {
        // Arrange & Act
        var settings = CreateSettings();

        // Assert
        settings.AppClientId.Should().Be("test-client-123");
    }

    [Fact]
    public void CognitoSettings_ShouldSetRegion()
    {
        // Arrange & Act
        var settings = CreateSettings();

        // Assert
        settings.Region.Should().Be("us-east-1");
    }

    [Fact]
    public void CognitoSettings_AllPropertiesShouldBeSettable()
    {
        // Arrange
        var settings = new CognitoSettings
        {
            UserPoolId = "pool1",
            AppClientId = "client1",
            Region = "eu-west-1"
        };

        // Act - Modify properties
        settings.UserPoolId = "pool2";
        settings.AppClientId = "client2";
        settings.Region = "ap-southeast-1";

        // Assert
        settings.UserPoolId.Should().Be("pool2");
        settings.AppClientId.Should().Be("client2");
        settings.Region.Should().Be("ap-southeast-1");
    }

    private CognitoSettings CreateSettings()
    {
        return new CognitoSettings
        {
            UserPoolId = "us-east-1_testpool",
            AppClientId = "test-client-123",
            Region = "us-east-1"
        };
    }
}
