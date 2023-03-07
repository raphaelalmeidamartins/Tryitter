using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Xunit;
using Tryitter.Models;

namespace Tryitter.Test.Models
{
  public class ImageTests
  {
    [Fact]
    public void Image_ShouldHaveProperties()
    {
      // Arrange
      var image = new Image();

      // Act

      // Assert
      image.Should().NotBeNull();
      image.Should().BeAssignableTo<Image>();
      image.FileName.Should().BeNull();
      image.ContentType.Should().BeNull();
      image.Data.Should().BeNull();
      image.Posts.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void Image_FileName_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(Image).GetProperty(nameof(Image.FileName));

      // Act
      var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), true);

      // Assert
      attribute.Should().NotBeNull().And.HaveCount(1);
    }

    [Fact]
    public void Image_FileName_ShouldHaveMaxLengthAttribute()
    {
      // Arrange
      var property = typeof(Image).GetProperty(nameof(Image.FileName));

      // Act
      var attribute = property?.GetCustomAttributes(typeof(MaxLengthAttribute), true);

      // Assert
      attribute.Should().NotBeNull().And.HaveCount(1);
      attribute.Should().BeOfType<MaxLengthAttribute[]>()
        .Which[0].Length.Should().Be(255);

    }

    [Fact]
    public void Image_ContentType_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(Image).GetProperty(nameof(Image.ContentType));

      // Act
      var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), true);

      // Assert
      attribute.Should().NotBeNull().And.HaveCount(1);
    }

    [Fact]
    public void Image_ContentType_ShouldHaveMaxLengthAttribute()
    {
      // Arrange
      var property = typeof(Image).GetProperty(nameof(Image.ContentType));

      // Act
      var attribute = property?.GetCustomAttributes(typeof(MaxLengthAttribute), true);

      // Assert
      attribute.Should().NotBeNull().And.HaveCount(1);
      attribute.Should().BeOfType<MaxLengthAttribute[]>()
          .Which[0].Length.Should().Be(100);
    }

    [Fact]
    public void Image_Data_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(Image).GetProperty(nameof(Image.Data));

      // Act
      var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), true);

      // Assert
      attribute.Should().NotBeNull().And.HaveCount(1);
    }

    [Fact]
    public void Image_Data_ShouldHaveMaxLengthAttribute()
    {
      // Arrange
      var property = typeof(Image).GetProperty(nameof(Image.Data));

      // Act
      var attribute = property?.GetCustomAttributes(typeof(MaxLengthAttribute), true);

      // Assert
      attribute.Should().NotBeNull().And.HaveCount(1);
      attribute.Should().BeOfType<MaxLengthAttribute[]>()
          .Which[0].Length.Should().Be(1048576);
    }

    [Fact]
    public void Image_Posts_ShouldBeEmptyCollection()
    {
      // Arrange
      var image = new Image();

      // Act

      // Assert
      image.Posts.Should().NotBeNull().And.BeEmpty();
    }
  }
}
