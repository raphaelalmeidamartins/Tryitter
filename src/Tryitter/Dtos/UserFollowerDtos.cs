using System.ComponentModel.DataAnnotations;

namespace Tryitter.Dtos.UserFollower
{
  public class CreateUserFollowerDto
  {
    [Required]
    public int FolloweeId { get; set; }
    [Required]
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

    [Required]
    public int FolloweeId { get; set; }
    [Required]
    public int FollowerId { get; set; }
  }
}
