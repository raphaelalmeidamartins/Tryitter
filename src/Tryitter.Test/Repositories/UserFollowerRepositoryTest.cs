using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading.Tasks;
using Tryitter.Models;
using Tryitter.Repositories;
using Xunit;
using Tryitter.Dtos.UserFollower;
using FluentAssertions;

namespace Tryitter.Test.Repositories
{
  public class UserFollowerRepositoryTest
  {
    private readonly TryitterContext _contextMock;
    private readonly IConfiguration _config;

    public UserFollowerRepositoryTest()
    {
      // Create a mock configuration
      _config = new ConfigurationBuilder().AddInMemoryCollection().Build();

      // Create mock DbContextOptions
      var options = new DbContextOptionsBuilder<TryitterContext>()
          .UseInMemoryDatabase(databaseName: "Tryitter4")
          .Options;

      var config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .Build();

      _contextMock = new TryitterContext(options, config);
    }

    [Fact]
    public async Task Create_WhenUsersAreValidAndNotAlreadyFollowing_ShouldReturnUserFollower()
    {
      // Arrange
      var modules = new List<Module>
      {
        new Module { Name = "Module 1" },
        new Module { Name = "Module 2" }
      };

      await this._contextMock.Modules.AddRangeAsync(modules);
      await _contextMock.SaveChangesAsync();

      var followee = new User()
      {
        Username = "testuser",
        Email = "testuser@example.com",
        PasswordHash = "testpassword",
        ModuleId = 1,
        Status = "Active",
        Bio = "Test bio",
        IsAdmin = false
      };
      var follower = new User()
      {
        Username = "testuser2",
        Email = "testuser2@example.com",
        PasswordHash = "testpassword2",
        ModuleId = 1,
        Status = "Active2",
        Bio = "Test bio2",
        IsAdmin = false
      };

      await this._contextMock.Users.AddRangeAsync(followee, followee);
      await _contextMock.SaveChangesAsync();

      var dto = new CreateUserFollowerDto { FolloweeId = followee.UserId, FollowerId = followee.UserId };

      var userFollowerRepository = new UserFollowerRepository(this._contextMock);

      // Act
      var result = await userFollowerRepository.Create(dto);

      // Assert
      result.Should().NotBeNull();
      result.FolloweeId.Should().Be(dto.FolloweeId);
      result.FollowerId.Should().Be(dto.FollowerId);
    }


    [Fact]
    public async Task Destroy_ShouldRemoveUserFollowerAndReturnIt()
    {
      // Arrange
      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var followeeId = 1;
      var followerId = 2;

      var userFollower = new UserFollower
      {
        FolloweeId = followeeId,
        FollowerId = followerId
      };

      this._contextMock.UserFollowers.Add(userFollower);
      await this._contextMock.SaveChangesAsync();

      var id = new UserFollowerIdDto
      {
        FolloweeId = followeeId,
        FollowerId = followerId
      };

      var userFollowerRepository = new UserFollowerRepository(this._contextMock);

      // Act
      var result = await userFollowerRepository.Destroy(id);

      // Assert
      result.Should().NotBeNull();
      result.FolloweeId.Should().Be(followeeId);
      result.FollowerId.Should().Be(followerId);

      this._contextMock.UserFollowers.Should().NotContain(userFollower);
    }
  }
}
