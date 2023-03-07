using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Xunit;
using System.Reflection;
using Tryitter.Dtos.UserFollower;

namespace Tryitter.Test.Dtos
{
  public class UserFollowerDtoTests
  {
    [Fact]
    public void CreateUserFollowerDto_FolloweeId_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(CreateUserFollowerDto).GetProperty(nameof(CreateUserFollowerDto.FolloweeId));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().NotBeNull();
    }

    [Fact]
    public void CreateUserFollowerDto_FollowerId_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(CreateUserFollowerDto).GetProperty(nameof(CreateUserFollowerDto.FollowerId));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().NotBeNull();
    }

    [Fact]
    public void UserFollowerIdDto_FolloweeId_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(UserFollowerIdDto).GetProperty(nameof(UserFollowerIdDto.FolloweeId));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().NotBeNull();
    }

    [Fact]
    public void UserFollowerIdDto_FollowerId_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(UserFollowerIdDto).GetProperty(nameof(UserFollowerIdDto.FollowerId));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().NotBeNull();
    }
  }
}
