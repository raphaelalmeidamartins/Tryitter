using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading.Tasks;
using Tryitter.Models;
using Tryitter.Repositories;
using Xunit;
using Tryitter.Dtos.User;
using FluentAssertions;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using System.IO;


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

      this._contextMock = new TryitterContext(options, config);
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
    public async Task CreateUser_ShouldThrowArgumentException_WhenUsernameIsNotAvailable()
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

      var user = new User()
      {
        Username = "johndoe",
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

      var dto = new CreateUserDto()
      {
        Username = "johndoe",
        Email = "johndoe@example.com",
        Password = "password",
        ModuleId = 1,
        Status = "active",
        Bio = "Hello, I am John Doe.",
      };

      // Acc
      var action = () => userRepository.Create(dto);

      // Assert
      await action.Should().ThrowAsync<ArgumentException>().WithMessage($"Username {dto.Username} already in use");
    }

    [Fact]
    public async Task CreateUser_ShouldThrowArgumentException_WhenEmailIsNotAvailable()
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

      var user = new User()
      {
        Username = "raphael",
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

      var dto = new CreateUserDto()
      {
        Username = "johndoe",
        Email = "testuser@example.com",
        Password = "password",
        ModuleId = 1,
        Status = "active",
        Bio = "Hello, I am John Doe.",
      };

      // Acc
      var action = () => userRepository.Create(dto);

      // Assert
      await action.Should().ThrowAsync<ArgumentException>().WithMessage($"Email {dto.Email} already in use");
    }

    [Fact]
    public async Task CreateUser_ShouldThrowArgumentException_WhenModuleIdIsNotValid()
    {
      // Arrang
      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var userRepository = new UserRepository(this._contextMock);

      var dto = new CreateUserDto()
      {
        Username = "johndoe",
        Email = "testuser@example.com",
        Password = "password",
        ModuleId = 1,
        Status = "active",
        Bio = "Hello, I am John Doe.",
      };

      // Acc
      var action = () => userRepository.Create(dto);

      // Assert
      await action.Should().ThrowAsync<ArgumentException>().WithMessage($"User was not created because Module with id {dto.ModuleId} was not found");
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
    public async Task FindMany_ShouldReturnListOfUsersWithTheProvidedModuleId_WhenUsersExist()
    {
      // Arrange

      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var users = new List<User>
      {
        new User { Username = "johndoe", Email = "johndoe@example.com", PasswordHash = "hash", Status = "active", Bio = "Wow", ModuleId = 1 },
        new User { Username = "janedoe", Email = "janedoe@example.com", PasswordHash = "hash", Status = "inactive", Bio = "Wow", ModuleId = 2 },
      };

      this._contextMock.Users.AddRange(users);
      this._contextMock.SaveChanges();

      var userRepository = new UserRepository(_contextMock);

      var dto = new FindManyUsersDto()
      {
        ModuleId = 1,
      };

      // Act
      var result = await userRepository.FindMany(dto);

      // Assert
      result.Should().NotBeNull();
      result.Should().HaveCount(1);
      result[0].Should().BeEquivalentTo(users[0]);
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
      result.Should().NotBeNull();
      result.UserId.Should().Be(user.UserId);
      result.Username.Should().BeEquivalentTo(user.Username);
      result.Email.Should().BeEquivalentTo(user.Email);
      result.ModuleId.Should().Be(user.ModuleId);
      result.Status.Should().BeEquivalentTo(user.Status);
      result.Bio.Should().BeEquivalentTo(user.Bio);
      result.IsAdmin.Should().BeFalse();
    }

    [Fact]
    public async Task FindById_WithInvalidId_ThrowArgumentExceptione()
    {
      // Arrange
      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var userRepository = new UserRepository(this._contextMock);

      // Acc
      var action = () => userRepository.FindById(1);

      // Assert
      await action.Should().ThrowAsync<ArgumentException>().WithMessage($"User with Id {1} not found");
    }


    [Fact]
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
      result.Should().NotBeNull();
      result.UserId.Should().Be(user.UserId);
      result.Username.Should().BeEquivalentTo(user.Username);
      result.Email.Should().BeEquivalentTo(user.Email);
      result.ModuleId.Should().Be(user.ModuleId);
      result.Status.Should().BeEquivalentTo(user.Status);
      result.Bio.Should().BeEquivalentTo(user.Bio);
      result.IsAdmin.Should().BeFalse();
    }

    [Fact]
    public async Task FindById_WithInvalidUsername_ThrowArgumentExceptione()
    {
      // Arrange
      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var userRepository = new UserRepository(this._contextMock);

      // Acc
      var action = () => userRepository.FindByUsername("gmail");

      // Assert
      await action.Should().ThrowAsync<ArgumentException>().WithMessage($"User with the username gmail not found");
    }

    [Fact]
    public async Task Destroy_Should_Remove_Module_From_Context()
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
      var result = await userRepository.Destroy(user.UserId);

      // Assert
      result.Should().BeEquivalentTo(user);
      this._contextMock.Users.Should().NotContain(user);
    }

    [Fact]
    public async Task Update_ShouldUpdateUserAndSaveChanges()
    {
      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var modules = new List<Module>
      {
        new Module { Name = "Module 1" },
        new Module { Name = "Module 2" }
      };

      await this._contextMock.Modules.AddRangeAsync(modules);
      await _contextMock.SaveChangesAsync();

      // Arrange
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
      this._contextMock.Users.Add(user);
      await this._contextMock.SaveChangesAsync();

      var dto = new UpdateUserDto
      {
        ModuleId = 2,
        Status = "inactive",
        Bio = "new bio",
        Password = "newpassword"
      };

      var userRepository = new UserRepository(this._contextMock);

      // Act
      var result = await userRepository.Update(user.UserId, dto);

      // Assert
      result.Should().NotBeNull();
      result.UserId.Should().Be(user.UserId);
      result.ModuleId.Should().Be(dto.ModuleId.Value);
      result.Status.Should().Be(dto.Status);
      result.Bio.Should().Be(dto.Bio);
      result.PasswordHash.Should().NotBeNullOrEmpty();

      var savedUser = await this._contextMock.Users.FindAsync(user.UserId);
      savedUser.Should().NotBeNull();
      savedUser?.UserId.Should().Be(user.UserId);
      savedUser?.ModuleId.Should().Be(dto.ModuleId.Value);
      savedUser?.Status.Should().Be(dto.Status);
      savedUser?.Bio.Should().Be(dto.Bio);
      savedUser?.PasswordHash.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task UpdateUser_ShouldThrowArgumentException_WhenModuleIdIsNotValid()
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

      // Arrange
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
      this._contextMock.Users.Add(user);
      await this._contextMock.SaveChangesAsync();

      var userRepository = new UserRepository(this._contextMock);

      var dto = new UpdateUserDto()
      {
        Password = "password",
        ModuleId = 3,
        Status = "active",
        Bio = "Hello, I am John Doe.",
      };

      // Acc
      var action = () => userRepository.Update(user.UserId, dto);

      // Assert
      await action.Should().ThrowAsync<ArgumentException>().WithMessage($"User was not updated because Module with id {dto.ModuleId} was not found");
    }
  }
}
