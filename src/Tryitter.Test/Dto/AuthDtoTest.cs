using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Tryitter.Dtos.Auth;
using Xunit;
using System.Reflection;

namespace Tryitter.Test.Dtos
{
  public class AuthDtoTest
  {
    [Fact]
    public void Username_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(AuthDto).GetProperty(nameof(AuthDto.Username));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().NotBeNull();
    }

    [Fact]
    public void Username_ShouldHaveStringLengthAttributeWithMaxLength50()
    {
      // Arrange
      var property = typeof(AuthDto).GetProperty(nameof(AuthDto.Username));

      // Act
      var attribute = property?.GetCustomAttribute<StringLengthAttribute>();

      // Assert
      attribute.Should().NotBeNull();
      attribute!.MaximumLength.Should().Be(50);
    }

    [Fact]
    public void Password_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(AuthDto).GetProperty(nameof(AuthDto.Password));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().NotBeNull();
    }

    [Fact]
    public void Password_ShouldHaveStringLengthAttributeWithMaxLength50()
    {
      // Arrange
      var property = typeof(AuthDto).GetProperty(nameof(AuthDto.Password));

      // Act
      var attribute = property?.GetCustomAttribute<StringLengthAttribute>();

      // Assert
      attribute.Should().NotBeNull();
      attribute!.MaximumLength.Should().Be(50);
    }
  }
}
