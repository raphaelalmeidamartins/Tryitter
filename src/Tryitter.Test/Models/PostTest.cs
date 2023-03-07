using FluentAssertions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;
using Tryitter.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tryitter.Test.Models
{
  public class PostTests
  {
    [Fact]
    public void Post_Content_ShouldHaveStringLengthAttribute()
    {
      // Arrange
      var property = typeof(Post).GetProperty(nameof(Post.Content));

      // Act
      var attribute = property?.GetCustomAttributes(typeof(StringLengthAttribute), true)
          .Cast<StringLengthAttribute>()
          .FirstOrDefault();

      // Assert
      attribute.Should().NotBeNull();
      attribute?.MaximumLength.Should().Be(300);
    }


    [Fact]
    public void Post_CreatedAt_ShouldBeSetToUtcNowByDefault()
    {
      // Arrange & Act
      var post = new Post();

      // Assert
      post.CreatedAt.Kind.Should().Be(DateTimeKind.Utc);
      post.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromSeconds(1));
    }


    [Fact]
    public void Post_UpdatedAt_ShouldBeSetToUtcNowByDefault()
    {
      // Arrange & Act
      var post = new Post();

      // Assert
      post.UpdatedAt.Kind.Should().Be(DateTimeKind.Utc);
      post.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromSeconds(1));
    }


    [Fact]
    public void Post_Image_ShouldAllowNull()
    {
      // Arrange
      var property = typeof(Post).GetProperty(nameof(Post.ImageId));

      // Act
      var attribute = property?.GetCustomAttributes(typeof(ForeignKeyAttribute), true);

      // Assert
      attribute.Should().NotBeNull().And.HaveCount(1);
      attribute.Should().BeOfType<ForeignKeyAttribute[]>()
          .Which.Single().Name.Should().Be("ImageId");
    }
  }
}
