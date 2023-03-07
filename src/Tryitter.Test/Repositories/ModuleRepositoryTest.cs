using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading.Tasks;
using Tryitter.Models;
using Tryitter.Repositories;
using Xunit;
using Tryitter.Dtos.Module;
using FluentAssertions;

namespace Tryitter.Test.Repositories
{
  public class ModuleRepositoryTest
  {
    private readonly TryitterContext _contextMock;
    private readonly IConfiguration _config;

    public ModuleRepositoryTest()
    {
      // Create a mock configuration
      _config = new ConfigurationBuilder().AddInMemoryCollection().Build();

      // Create mock DbContextOptions
      var options = new DbContextOptionsBuilder<TryitterContext>()
          .UseInMemoryDatabase(databaseName: "Tryitter3")
          .Options;

      var config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .Build();

      _contextMock = new TryitterContext(options, config);
    }

    [Fact]
    public async Task Create_ShouldReturnModule_WhenValidDtoProvided()
    {
      // Arrang
      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var moduleRepository = new ModuleRepository(this._contextMock);

      var dto = new CreateModuleDto()
      {
        Name = "Aceleração de C#"
      };

      // Act
      var result = await moduleRepository.Create(dto);

      // Assert
      result.Should().NotBeNull();
      result.Name.Should().BeEquivalentTo(dto.Name);
    }

    [Fact]
    public async Task Update_ShouldReturnModule_WhenValidDtoProvided()
    {
      // Arrang
      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var moduleRepository = new ModuleRepository(this._contextMock);

      var module = new Module { Name = "Module 1" };

      this._contextMock.Modules.Add(module);
      this._contextMock.SaveChanges();

      var dto = new UpdateModuleDto()
      {
        Name = "Aceleração de C#"
      };

      // Act
      var result = await moduleRepository.Update(module.ModuleId, dto);

      // Assert
      result.Should().NotBeNull();
      result.Name.Should().BeEquivalentTo(dto.Name);
    }

    [Fact]
    public async Task Destroy_Should_Remove_Module_From_Context()
    {
      // Arrange
      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var module = new Module { Name = "Test Module" };
      this._contextMock.Modules.Add(module);
      await this._contextMock.SaveChangesAsync();

      var moduleRepository = new ModuleRepository(this._contextMock);

      // Act
      var result = await moduleRepository.Destroy(module.ModuleId);

      // Assert
      result.Should().BeEquivalentTo(module);
      this._contextMock.Modules.Should().NotContain(module);
    }


    [Fact]
    public async Task FindMany_ShouldReturnListOfModules_WhenPostsExist()
    {
      // Arrange

      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var modules = new List<Module>
      {
        new Module { Name = "Aceleração de C#" },
        new Module { Name = "Aceleração de Java" },
      };

      this._contextMock.Modules.AddRange(modules);
      this._contextMock.SaveChanges();

      var moduleRepository = new ModuleRepository(_contextMock);

      // Act
      var result = await moduleRepository.FindMany(null);

      // Assert
      result.Should().NotBeNull();
      result.Should().HaveCount(modules.Count);
      result.Select(m => m.ModuleId).Should().BeEquivalentTo(modules.Select(m => m.ModuleId));
      result.Select(m => m.Name).Should().BeEquivalentTo(modules.Select(m => m.Name));
    }

    [Fact]
    public async Task FindById_WithValidId_ReturnsModule()
    {
      // Arrange
      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var module = new Module()
      {
        Name = "Aceleração de C#",
      };

      await this._contextMock.Modules.AddAsync(module);
      await this._contextMock.SaveChangesAsync();

      var moduleRepository = new ModuleRepository(this._contextMock);

      // Act
      var result = await moduleRepository.FindById(1);

      // Assert
      result.Should().NotBeNull();
      result.ModuleId.Should().Be(module.ModuleId);
      result.Name.Should().BeEquivalentTo(module.Name);
    }

    [Fact]
    public async Task FindById_WithInvalidId_ThrowArgumentExceptione()
    {
      // Arrange
      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var moduleRepository = new ModuleRepository(this._contextMock);

      // Acc
      var action = () => moduleRepository.FindById(1);

      // Assert
      await action.Should().ThrowAsync<ArgumentException>().WithMessage($"Module with Id {1} not found");
    }
  }
}
