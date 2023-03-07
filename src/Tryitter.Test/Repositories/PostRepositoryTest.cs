using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading.Tasks;
using Tryitter.Models;
using Tryitter.Repositories;
using Xunit;
using Tryitter.Dtos.Post;
using FluentAssertions;

namespace Tryitter.Test.Repositories
{
  public class PostRepositoryTest
  {
    private readonly TryitterContext _contextMock;
    private readonly IConfiguration _config;

    public PostRepositoryTest()
    {
      // Create a mock configuration
      _config = new ConfigurationBuilder().AddInMemoryCollection().Build();

      // Create mock DbContextOptions
      var options = new DbContextOptionsBuilder<TryitterContext>()
          .UseInMemoryDatabase(databaseName: "Tryitter2")
          .Options;

      var config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .Build();

      _contextMock = new TryitterContext(options, config);
    }

    [Fact]
    public async Task CreatePost_ShouldReturnPost_WhenValidDtoProvided()
    {
      // Arrang
      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var author = new User()
      {
        Username = "testuser",
        Email = "testuser@example.com",
        PasswordHash = "testpassword",
        ModuleId = 1,
        Status = "Active",
        Bio = "Test bio",
        IsAdmin = false
      };
      await this._contextMock.Users.AddAsync(author);
      await this._contextMock.SaveChangesAsync();

      var postRepository = new PostRepository(this._contextMock);

      var dto = new CreatePostDto()
      {
        AuthorId = author.UserId,
        Content = "Any text"
      };

      // Act
      var result = await postRepository.Create(dto);

      // Assert
      result.Should().NotBeNull();
      result.AuthorId.Should().Be(dto.AuthorId);
      result.Content.Should().Be(dto.Content);
    }

    [Fact]
    public async Task FindMany_ShouldReturnListOfPosts_WhenPostsExist()
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

      var posts = new List<Post>
      {
        new Post { AuthorId = 1, Content = "Lorem ispsum" },
        new Post { AuthorId = 1, Content = "Lorem ispsum" },
      };

      this._contextMock.Posts.AddRange(posts);
      this._contextMock.SaveChanges();

      var postRepository = new PostRepository(_contextMock);

      // Act
      var result = await postRepository.FindMany(null);

      // Assert
      result.Should().NotBeNull();
      result.Should().HaveCount(users.Count);
      result.Select(p => p.PostId).Should().BeEquivalentTo(posts.Select(p => p.PostId));
      result.Select(p => p.AuthorId).Should().BeEquivalentTo(posts.Select(p => p.AuthorId));
      result.Select(p => p.Content).Should().BeEquivalentTo(posts.Select(p => p.Content));
    }

    [Fact]
    public async Task FindMany_ShouldReturnListOfUsersWithTheProvidedModuleId_WhenUsersExist()
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

      var posts = new List<Post>
      {
        new Post { AuthorId = 1, Content = "Lorem ispsum" },
        new Post { AuthorId = 2, Content = "Lorem ispsum" },
      };

      this._contextMock.Posts.AddRange(posts);
      this._contextMock.SaveChanges();

      var postRepository = new PostRepository(_contextMock);

      var dto = new FindManyPostsDto()
      {
        AuthorId = 1,
      };

      // Act
      var result = await postRepository.FindMany(dto);

      // Assert
      result.Should().NotBeNull();
      result.Should().HaveCount(1);
      result[0].Should().BeEquivalentTo(posts[0]);
    }

    [Fact]
    public async Task FindById_WithValidId_ReturnsPost()
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

      var post = new Post()
      {
        AuthorId = user.UserId,
        Content = "Lorem ipsum",
      };

      await this._contextMock.Posts.AddAsync(post);
      await this._contextMock.SaveChangesAsync();

      var postRepository = new PostRepository(this._contextMock);

      // Act
      var result = await postRepository.FindById(user.UserId);

      // Assert
      result.Should().NotBeNull();
      result.PostId.Should().Be(post.PostId);
      result.AuthorId.Should().Be(post.AuthorId);
      result.Content.Should().Be(post.Content);
    }

    [Fact]
    public async Task FindById_WithInvalidId_ThrowArgumentExceptione()
    {
      // Arrange
      this._contextMock.Database.EnsureDeleted();
      this._contextMock.Database.EnsureCreated();

      var postRepository = new PostRepository(this._contextMock);

      // Acc
      var action = () => postRepository.FindById(1);

      // Assert
      await action.Should().ThrowAsync<ArgumentException>().WithMessage($"Post with Id {1} not found");
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

      var post = new Post()
      {
        AuthorId = user.UserId,
        Content = "Lorem ipsum",
      };

      await this._contextMock.Posts.AddAsync(post);
      await this._contextMock.SaveChangesAsync();

      var postRepository = new PostRepository(this._contextMock);

      // Act
      var result = await postRepository.Destroy(post.PostId);

      // Assert
      result.Should().BeEquivalentTo(post);
      this._contextMock.Posts.Should().NotContain(post);
    }

    [Fact]
    public async Task Update_ShouldReturnModule_WhenValidDtoProvided()
    {
      // Arrang
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

      var post = new Post()
      {
        AuthorId = user.UserId,
        Content = "Lorem ipsum",
      };

      await this._contextMock.Posts.AddAsync(post);
      await this._contextMock.SaveChangesAsync();

      var postRepository = new PostRepository(this._contextMock);

      var dto = new UpdatePostDto()
      {
        Content = "Aceleração de C# uhull"
      };

      // Act
      var result = await postRepository.Update(post.PostId, dto);

      // Assert
      result.Should().NotBeNull();
      result.Content.Should().BeEquivalentTo(dto.Content);
    }
  }
}
