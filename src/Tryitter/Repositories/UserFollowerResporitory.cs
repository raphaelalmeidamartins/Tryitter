using Tryitter.Models;
using Microsoft.EntityFrameworkCore;
using Tryitter.Dtos.UserFollower;

namespace Tryitter.Repositories;

public class UserFollowerRepository : IUserFollowerRepository
{
  private readonly TryitterContext _context;

  public UserFollowerRepository(TryitterContext context)
  {
    this._context = context;
  }

  public async Task<UserFollower> Create(CreateUserFollowerDto dto)
  {
    var followee = await this._context.Users.FindAsync(dto.FolloweeId);
    if (followee == null) throw new ArgumentException($"User with id {dto.FolloweeId} not found");
    var follower = await this._context.Users.FindAsync(dto.FollowerId);
    if (follower == null) throw new ArgumentException($"User with id {dto.FollowerId} not found");

    var isAlreadyFollowing = this._context.UserFollowers.Where(uf => uf.FolloweeId == dto.FolloweeId && uf.FollowerId == dto.FollowerId).Any();
    if (isAlreadyFollowing != false) throw new ArgumentException("User is already being followed");
    Console.Write(isAlreadyFollowing);
    
    var userFollower = new UserFollower()
    {
      FolloweeId = dto.FolloweeId,
      Followee = followee,
      FollowerId = dto.FollowerId,
      Follower = follower,

    };
  
    await this._context.UserFollowers.AddAsync(userFollower);
    await this._context.SaveChangesAsync();
    await this._context.Entry(userFollower).ReloadAsync();
    return userFollower;
  }

  public async Task<UserFollower> Destroy(UserFollowerIdDto id)
  {
    var userFollower = await this._context.UserFollowers
      .FirstOrDefaultAsync(uf => uf.FolloweeId == id.FolloweeId && uf.FollowerId == id.FollowerId);

    if (userFollower == null)
    {
      throw new ArgumentException($"The users are not following each other");
    }

    this._context.UserFollowers.Remove(userFollower);
    await this._context.SaveChangesAsync();

    return userFollower;
  }
}
