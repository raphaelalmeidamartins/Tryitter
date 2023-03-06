using Tryitter.Models;
using Tryitter.Dtos.Post;

namespace Tryitter.Repositories;

public interface IPostRepository : IRepository<Post, CreatePostDto, UpdatePostDto, FindManyPostsDto, int>
{
}
