using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentAssertions;
using Xunit;
using System.Reflection;
using Tryitter.Models;

namespace Tryitter.Test.Models
{
  public class UserFollowerTests
  {
    [Fact]
    public void UserFollower_FolloweeId_ShouldHaveForeignKeyAttribute()
    {
      // Arrange
      var property = typeof(UserFollower).GetProperty(nameof(UserFollower.FolloweeId));

      // Act
      var attribute = property?.GetCustomAttribute<ForeignKeyAttribute>();

      // Assert
      attribute.Should().NotBeNull();
      attribute!.Name.Should().Be("FolloweeId");
    }


    [Fact]
    public void UserFollower_FollowerId_ShouldHaveForeignKeyAttribute()
    {
      // Arrange
      var property = typeof(UserFollower).GetProperty(nameof(UserFollower.FollowerId));

      // Act
      var attribute = property?.GetCustomAttribute<ForeignKeyAttribute>();

      // Assert
      attribute.Should().NotBeNull();
      attribute!.Name.Should().Be("FollowerId");
    }
  }
}
