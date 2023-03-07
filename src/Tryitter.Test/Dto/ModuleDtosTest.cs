using System.ComponentModel.DataAnnotations;
using Tryitter.Dtos.Module;
using Xunit;
using System.Reflection;
using FluentAssertions;

namespace Tryitter.Test.Dtos
{
  public class ModuleDtosTest
  {
    [Fact]
    public void CreateModuleDto_Name_ShouldHaveMaxLengthAttribute()
    {
      // Arrange
      var property = typeof(CreateModuleDto).GetProperty(nameof(CreateModuleDto.Name));

      // Act
      var attribute = property?.GetCustomAttribute<MaxLengthAttribute>();

      // Assert
      attribute.Should().NotBeNull();
      attribute!.Length.Should().Be(255);
    }

    [Fact]
    public void CreateModuleDto_Name_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(CreateModuleDto).GetProperty(nameof(CreateModuleDto.Name));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().NotBeNull();
    }

    [Fact]
    public void UpdateModuleDto_Name_ShouldHaveMaxLengthAttribute()
    {
      // Arrange
      var property = typeof(UpdateModuleDto).GetProperty(nameof(UpdateModuleDto.Name));

      // Act
      var attribute = property?.GetCustomAttribute<MaxLengthAttribute>();

      // Assert
      attribute.Should().NotBeNull();
      attribute!.Length.Should().Be(255);
    }

    [Fact]
    public void UpdateModuleDto_Name_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(UpdateModuleDto).GetProperty(nameof(UpdateModuleDto.Name));

      // Act
      var attribute = property?.GetCustomAttribute<RequiredAttribute>();

      // Assert
      attribute.Should().NotBeNull();
    }

    [Fact]
    public void FindManyModulesDto_ShouldNotHaveProperties()
    {
      // Arrange
      var properties = typeof(FindManyModulesDto).GetProperties();

      // Assert
      properties.Should().BeEmpty();
    }
  }
}
