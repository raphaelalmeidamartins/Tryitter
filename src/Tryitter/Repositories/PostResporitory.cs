using Tryitter.Models;
using Microsoft.EntityFrameworkCore;
using Tryitter.Dtos.Post;
using Tryitter.Utils;

namespace Tryitter.Repositories;

public class PostRepository : IPostRepository
{
  private readonly TryitterContext _context;

  public PostRepository(TryitterContext context)
  {
    this._context = context;
  }

  public async Task<Post> Create(CreatePostDto dto)
  {
    var author = await this._context.Users.FindAsync(dto.AuthorId);

    if (author == null) throw new ArgumentException($"Author with id {dto.AuthorId} not found");

    var post = new Post()
    {
      Content = dto.Content,
      Author = author,
      AuthorId = author.UserId,
    };

    if (dto.Image != null)
    {
      var postImage = await ImageDecoder.GetFileData(dto.Image);
      var image = new Image()
      {
        FileName = dto.Image.FileName,
        ContentType = dto.Image.ContentType,
        Data = postImage,
      };
      await this._context.Images.AddAsync(image);
      await this._context.SaveChangesAsync();

      post.Image = image;
      post.ImageId = image.ImageId;
    }

    await this._context.Posts.AddAsync(post);
    await this._context.SaveChangesAsync();
    await this._context.Entry(post).ReloadAsync();
    return post;
  }

  public async Task<List<Post>> FindMany(FindManyPostsDto? dto)
  {
    IQueryable<Post> query = this._context.Posts;

    if (dto?.AuthorId != null)
    {
      query = query.Where(p => p.AuthorId == dto.AuthorId);
    }

    var posts = await query
      .Include(p => p.Author)
      .ToListAsync();

    return posts;
  }

  public async Task<Post> FindById(int id)
  {
    var entry = await this._context.Posts
      .Include(p => p.Author)
      .FirstOrDefaultAsync(p => p.PostId == id);

    if (entry == null)
    {
      throw new ArgumentException($"Post with Id {id} not found");
    }

    return entry!;
  }

  public async Task<Post> Update(int id, UpdatePostDto dto)
  {
    var post = await this.FindById(id);

    post.Content = dto.Content;

    await this._context.SaveChangesAsync();

    return post;
  }

  public async Task<Post> Destroy(int id)
  {
    var posts = await this.FindById(id);
    this._context.Posts.Remove(posts);

    await this._context.SaveChangesAsync();

    return posts;
  }
}
