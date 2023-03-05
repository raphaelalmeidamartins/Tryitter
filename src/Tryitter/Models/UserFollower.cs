using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tryitter.Models;

public class UserFollower
{
  [ForeignKey("FolloweeId")]
  public int FolloweeId { get; set; }
  public User Followee { get; set; } = default!;

  [ForeignKey("FollowerId")]
  public int FollowerId { get; set; }
  public User Follower { get; set; } = default!;
}
