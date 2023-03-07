using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Tryitter.Dtos.Post;
using Xunit;
using System.Reflection;
using FluentAssertions;

namespace Tryitter.Test.Dtos
{
  public class PostDtosTest
  {
    [Fact]
    public void CreatePostDto_ShouldHaveRequiredAuthorId()
    {
      // Arrange
      var property = typeof(CreatePostDto).GetProperty(nameof(CreatePostDto.AuthorId));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().NotBeNull();
    }

    [Fact]
    public void CreatePostDto_ShouldHaveRequiredContent()
    {
      // Arrange
      var property = typeof(CreatePostDto).GetProperty(nameof(CreatePostDto.Content));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().NotBeNull();
    }

    [Fact]
    public void CreatePostDto_ShouldHaveStringLengthAttributeOnContent()
    {
      // Arrange
      var property = typeof(CreatePostDto).GetProperty(nameof(CreatePostDto.Content));

      // Act
      var attribute = property?.GetCustomAttribute<StringLengthAttribute>();

      // Assert
      attribute.Should().NotBeNull();
      attribute!.MaximumLength.Should().Be(300);
    }

    [Fact]
    public void CreatePostDto_ShouldHaveOptionalImage()
    {
      // Arrange
      var property = typeof(CreatePostDto).GetProperty(nameof(CreatePostDto.Image));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().BeNull();
    }

    [Fact]
    public void UpdatePostDto_ShouldHaveRequiredContent()
    {
      // Arrange
      var property = typeof(UpdatePostDto).GetProperty(nameof(UpdatePostDto.Content));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().NotBeNull();
    }

    [Fact]
    public void UpdatePostDto_ShouldHaveStringLengthAttributeOnContent()
    {
      // Arrange
      var property = typeof(UpdatePostDto).GetProperty(nameof(UpdatePostDto.Content));

      // Act
      var attribute = property?.GetCustomAttribute<StringLengthAttribute>();

      // Assert
      attribute.Should().NotBeNull();
      attribute!.MaximumLength.Should().Be(300);
    }

    [Fact]
    public void FindManyPostsDto_ShouldHaveNullableAuthorId()
    {
      // Arrange
      var property = typeof(FindManyPostsDto).GetProperty(nameof(FindManyPostsDto.AuthorId));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().BeNull();
    }
  }
}
