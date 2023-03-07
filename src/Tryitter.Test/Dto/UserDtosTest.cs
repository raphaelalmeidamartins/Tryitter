using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Tryitter.Dtos.User;
using Xunit;
using System.Reflection;

namespace Tryitter.Test.Dtos
{
  public class UserDtosTest
  {
    [Fact]
    public void CreateUserDto_Username_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(CreateUserDto).GetProperty(nameof(CreateUserDto.Username));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().NotBeNull();
    }

    [Fact]
    public void CreateUserDto_Username_ShouldHaveStringLengthAttribute()
    {
      // Arrange
      var property = typeof(CreateUserDto).GetProperty(nameof(CreateUserDto.Username));

      // Act
      var attribute = property?.GetCustomAttribute<StringLengthAttribute>();

      // Assert
      attribute.Should().NotBeNull();
      attribute!.MaximumLength.Should().Be(50);
    }

    [Fact]
    public void CreateUserDto_Email_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(CreateUserDto).GetProperty(nameof(CreateUserDto.Email));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().NotBeNull();
    }

    [Fact]
    public void CreateUserDto_Email_ShouldHaveStringLengthAttribute()
    {
      // Arrange
      var property = typeof(CreateUserDto).GetProperty(nameof(CreateUserDto.Email));

      // Act
      var attribute = property?.GetCustomAttribute<StringLengthAttribute>();

      // Assert
      attribute.Should().NotBeNull();
      attribute!.MaximumLength.Should().Be(50);
    }

    [Fact]
    public void CreateUserDto_Password_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(CreateUserDto).GetProperty(nameof(CreateUserDto.Password));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().NotBeNull();
    }

    [Fact]
    public void CreateUserDto_Password_ShouldHaveStringLengthAttribute()
    {
      // Arrange
      var property = typeof(CreateUserDto).GetProperty(nameof(CreateUserDto.Password));

      // Act
      var attribute = property?.GetCustomAttribute<StringLengthAttribute>();

      // Assert
      attribute.Should().NotBeNull();
      attribute!.MaximumLength.Should().Be(50);
    }

    [Fact]
    public void CreateUserDto_ModuleId_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(CreateUserDto).GetProperty(nameof(CreateUserDto.ModuleId));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().NotBeNull();
    }

    [Fact]
    public void CreateUserDto_Status_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(CreateUserDto).GetProperty(nameof(CreateUserDto.Status));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().NotBeNull();
    }

    [Fact]
    public void CreateUserDto_Status_ShouldHaveStringLengthAttribute()
    {
      // Arrange
      var property = typeof(CreateUserDto).GetProperty(nameof(CreateUserDto.Status));

      // Act
      var attribute = property?.GetCustomAttribute<StringLengthAttribute>();

      // Assert
      attribute.Should().NotBeNull();
      attribute!.MaximumLength.Should().Be(100);
    }

    [Fact]
    public void CreateUserDto_Bio_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(CreateUserDto).GetProperty(nameof(CreateUserDto.Bio));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().BeNull();
    }


    [Fact]
    public void CreateUserDto_Bio_ShouldHaveStringLengthAttribute()
    {
      // Arrange
      var property = typeof(CreateUserDto).GetProperty(nameof(CreateUserDto.Bio));

      // Act
      var attribute = property?.GetCustomAttribute<StringLengthAttribute>();

      // Assert
      attribute.Should().NotBeNull();
      attribute!.MaximumLength.Should().Be(300);
    }

    [Fact]
    public void UpdateUserDto_Password_ShouldAllowNull()
    {
      // Arrange
      var property = typeof(UpdateUserDto).GetProperty(nameof(UpdateUserDto.Password));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().BeNull();
    }

    [Fact]
    public void UpdateUserDto_ModuleId_ShouldAllowNull()
    {
      // Arrange
      var property = typeof(UpdateUserDto).GetProperty(nameof(UpdateUserDto.ModuleId));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().BeNull();
    }

    [Fact]
    public void UpdateUserDto_Status_ShouldAllowNull()
    {
      // Arrange
      var property = typeof(UpdateUserDto).GetProperty(nameof(UpdateUserDto.Status));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().BeNull();
    }

    [Fact]
    public void UpdateUserDto_Bio_ShouldAllowNull()
    {
      // Arrange
      var property = typeof(UpdateUserDto).GetProperty(nameof(UpdateUserDto.Bio));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().BeNull();
    }

    [Fact]
    public void UpdateUserDto_Bio_ShouldHaveStringLengthAttribute()
    {
      // Arrange
      var property = typeof(UpdateUserDto).GetProperty(nameof(UpdateUserDto.Bio));

      // Act
      var attribute = property?.GetCustomAttribute<StringLengthAttribute>();

      // Assert
      attribute.Should().NotBeNull();
      attribute!.MaximumLength.Should().Be(300);
    }

    [Fact]
    public void UpdateUserDto_ProfilePicture_ShouldAllowNull()
    {
      // Arrange
      var property = typeof(UpdateUserDto).GetProperty(nameof(UpdateUserDto.ProfilePicture));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().BeNull();
    }

    [Fact]
    public void FindManyUsersDto_ModuleId_CanBeNull()
    {
      // Arrange
      var property = typeof(FindManyUsersDto).GetProperty(nameof(FindManyUsersDto.ModuleId));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // ˝Assert
      attribute.Should().BeNull();
    }
  }
}
