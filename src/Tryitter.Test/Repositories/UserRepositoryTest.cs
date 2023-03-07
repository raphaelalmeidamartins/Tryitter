using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading.Tasks;
using Tryitter.Models;
using Tryitter.Repositories;
using Xunit;
using Tryitter.Dtos.User;
using FluentAssertions;

namespace Tryitter.Test.Repositories
{
  public class UserRepositoryTest
  {
    private readonly TryitterContext _contextMock;
    private readonly IConfiguration _config;

    public UserRepositoryTest()
    {
      // Create a mock configuration
      _config = new ConfigurationBuilder().AddInMemoryCollection().Build();

      // Create mock DbContextOptions
      var options = new DbContextOptionsBuilder<TryitterContext>()
          .UseInMemoryDatabase(databaseName: "Tryitter")
          .Options;

      var config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .Build();

      _contextMock = new TryitterContext(options, config);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnUser_WhenValidDtoProvided()
    {
      // Arrang
      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var modules = new List<Module>
      {
        new Module { Name = "Module 1" },
        new Module { Name = "Module 2" }
      };

      await this._contextMock.Modules.AddRangeAsync(modules);
      await _contextMock.SaveChangesAsync();

      var userRepository = new UserRepository(this._contextMock);

      var dto = new CreateUserDto()
      {
        Username = "johndoe",
        Email = "johndoe@example.com",
        Password = "password",
        ModuleId = 1,
        Status = "active",
        Bio = "Hello, I am John Doe.",
      };

      // Act
      var result = await userRepository.Create(dto);

      // Assert
      result.Should().NotBeNull();
      result.Username.Should().Be(dto.Username);
      result.Email.Should().Be(dto.Email);
      result.ModuleId.Should().Be(dto.ModuleId);
      result.Status.Should().Be(dto.Status);
      result.Bio.Should().Be(dto.Bio);
    }

    [Fact]
    public async Task FindMany_ShouldReturnListOfUsers_WhenUsersExist()
    {
      // Arrange

      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var users = new List<User>
      {
        new User { Username = "johndoe", Email = "johndoe@example.com", PasswordHash = "hash", Status = "active", Bio = "Wow" },
        new User { Username = "janedoe", Email = "janedoe@example.com", PasswordHash = "hash", Status = "inactive", Bio = "Wow" },
      };

      this._contextMock.Users.AddRange(users);
      this._contextMock.SaveChanges();

      var userRepository = new UserRepository(_contextMock);

      // Act
      var result = await userRepository.FindMany(null);

      // Assert
      result.Should().NotBeNull();
      result.Should().HaveCount(users.Count);
      result.Select(u => u.UserId).Should().BeEquivalentTo(users.Select(u => u.UserId));
      result.Select(u => u.Username).Should().BeEquivalentTo(users.Select(u => u.Username));
      result.Select(u => u.Email).Should().BeEquivalentTo(users.Select(u => u.Email));
      result.Select(u => u.Status).Should().BeEquivalentTo(users.Select(u => u.Status));
    }

    [Fact]
    public async Task FindById_WithValidId_ReturnsUser()
    {
      // Arrange
      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var user = new User()
      {
        Username = "testuser",
        Email = "testuser@example.com",
        PasswordHash = "testpassword",
        ModuleId = 1,
        Status = "Active",
        Bio = "Test bio",
        IsAdmin = false
      };
      await this._contextMock.Users.AddAsync(user);
      await this._contextMock.SaveChangesAsync();

      var userRepository = new UserRepository(this._contextMock);

      // Act
      var result = await userRepository.FindById(user.UserId);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(user.UserId, result.UserId);
      Assert.Equal(user.Username, result.Username);
      Assert.Equal(user.Email, result.Email);
      Assert.Equal(user.ModuleId, result.ModuleId);
      Assert.Equal(user.Status, result.Status);
      Assert.Equal(user.Bio, result.Bio);
      Assert.False(result.IsAdmin);
    }


    public async Task FindByUsername_WithValidId_ReturnsUser()
    {
      // Arrange
      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var user = new User()
      {
        Username = "testuser",
        Email = "testuser@example.com",
        PasswordHash = "testpassword",
        ModuleId = 1,
        Status = "Active",
        Bio = "Test bio",
        IsAdmin = false
      };
      await this._contextMock.Users.AddAsync(user);
      await this._contextMock.SaveChangesAsync();

      var userRepository = new UserRepository(this._contextMock);

      // Act
      var result = await userRepository.FindByUsername(user.Username);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(user.UserId, result.UserId);
      Assert.Equal(user.Username, result.Username);
      Assert.Equal(user.Email, result.Email);
      Assert.Equal(user.ModuleId, result.ModuleId);
      Assert.Equal(user.Status, result.Status);
      Assert.Equal(user.Bio, result.Bio);
      Assert.False(result.IsAdmin);
    }
  }
}
