using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using FluentAssertions;
using Tryitter.Models;
using Xunit;

namespace Tryitter.Test.Models
{
  public class ModuleTests
  {
    [Fact]
    public void Module_ModuleId_ShouldHavePrivateSetter()
    {
      // Arrange
      var property = typeof(Module).GetProperty(nameof(Module.ModuleId));

      // Act
      var setter = property?.GetSetMethod(true);

      // Assert
      setter.Should().NotBeNull();
    }

    [Fact]
    public void Module_Name_ShouldHaveRequiredAttribute()
    {
      // Arrange
      var property = typeof(Module).GetProperty(nameof(Module.Name));

      // Act
      var attribute = property?.GetCustomAttributes(typeof(RequiredAttribute), true);

      // Assert
      attribute.Should().NotBeNull().And.HaveCount(1);
    }

    [Fact]
    public void Module_Name_ShouldHaveMaxLengthAttribute()
    {
      // Arrange
      var property = typeof(Module).GetProperty(nameof(Module.Name));

      // Act
      var attribute = property?.GetCustomAttributes(typeof(MaxLengthAttribute), true);

      // Assert
      attribute.Should().NotBeNull().And.HaveCount(1);
      attribute.Should().BeOfType<MaxLengthAttribute[]>()
          .Which[0].Length.Should().Be(255);
    }

    [Fact]
    public void Module_Users_ShouldBeInitializedAsEmptyHashSet()
    {
      // Arrange
      var module = new Module();

      // Act
      var users = module.Users;

      // Assert
      users.Should().NotBeNull().And.BeEmpty();
      users.Should().BeOfType<HashSet<User>>();
    }

    [Fact]
    public void Module_Users_ShouldBeMutable()
    {
      // Arrange
      var module = new Module();
      var user = new User();

      // Act
      module.Users.Add(user);

      // Assert
      module.Users.Should().Contain(user);
    }

    [Fact]
    public void Module_Users_ShouldBeIgnoredByJsonSerializer()
    {
      // Arrange
      var module = new Module();

      // Act
      var json = JsonSerializer.Serialize(module);

      // Assert
      json.Should().NotContain("Users");
    }
  }
}
