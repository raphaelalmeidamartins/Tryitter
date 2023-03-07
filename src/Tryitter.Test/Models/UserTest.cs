using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xunit;
using Tryitter.Models;

namespace Tryitter.Test.Models
{
  public class UserTests
  {
    [Fact]
    public void User_Username_ShouldHaveStringLengthAttribute()
    {
      // Arrange
      var property = typeof(User).GetProperty(nameof(User.Username));

      // Act
      var attribute = property?.GetCustomAttributes(typeof(StringLengthAttribute), true);

      // Assert
      attribute.Should().NotBeNull().And.HaveCount(1);
      attribute.Should().BeOfType<StringLengthAttribute[]>()
          .Which.Single().MaximumLength.Should().Be(50);
    }


    [Fact]
    public void User_Email_ShouldHaveStringLengthAttribute()
    {
      // Arrange
      var property = typeof(User).GetProperty(nameof(User.Email));

      // Act
      var attribute = property?.GetCustomAttributes(typeof(StringLengthAttribute), true);

      // Assert
      attribute.Should().NotBeNull().And.HaveCount(1);
      attribute.Should().BeOfType<StringLengthAttribute[]>()
          .Which.Single().MaximumLength.Should().Be(50);
    }

    [Fact]
    public void User_PasswordHash_ShouldBeRequired()
    {
      // Arrange
      var property = typeof(User).GetProperty(nameof(User.PasswordHash));

      // Act
      var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), true);

      // Assert
      attribute.Should().NotBeNull().And.HaveCount(1);
    }

    [Fact]
    public void User_ModuleId_ShouldBeRequired()
    {
      // Arrange
      var property = typeof(User).GetProperty(nameof(User.ModuleId));

      // Act
      var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), true);

      // Assert
      attribute.Should().NotBeNull().And.HaveCount(1);
    }

    [Fact]
    public void User_Status_ShouldHaveStringLengthAttribute()
    {
      // Arrange
      var property = typeof(User).GetProperty(nameof(User.Status));

      // Act
      var attribute = property?.GetCustomAttributes(typeof(StringLengthAttribute), true).FirstOrDefault() as StringLengthAttribute;

      // Assert
      attribute.Should().NotBeNull();
      attribute?.MaximumLength.Should().Be(100);
    }


    [Fact]
    public void User_Bio_ShouldAllowNull()
    {
      // Arrange
      var property = typeof(User).GetProperty(nameof(User.Bio));

      // Act
      var attribute = property?.GetCustomAttributes(typeof(StringLengthAttribute), true);

      // Assert
      attribute.Should().NotBeNull().And.HaveCount(1);
      attribute.Should().BeOfType<StringLengthAttribute[]>()
          .Which.Single().MaximumLength.Should().Be(300);
    }

    [Fact]
    public void User_IsAdmin_ShouldHaveDefaultValue()
    {
      // Arrange & Act
      var user = new User();

      // Assert
      user.IsAdmin.Should().BeFalse();
    }
  }
}
