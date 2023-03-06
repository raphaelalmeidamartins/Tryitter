using System.ComponentModel.DataAnnotations;

namespace Tryitter.Dtos.UserFollower
{
  public class CreateUserFollowerDto
  {
    public int FolloweeId { get; set; }
    public int FollowerId { get; set; }
  }
  public class UpdateUserFollowerDto
  {
  }

  public class FindManyUserFollowersDto
  {
  }

  public class UserFollowerIdDto
  {
    public int FolloweeId { get; set; }
    public int FollowerId { get; set; }
  }
}
