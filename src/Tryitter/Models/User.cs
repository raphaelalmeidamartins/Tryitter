using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tryitter.Models;

public class User
{
  public int UserId { get; private set; }

  [Required]
  [StringLength(50)]
  public string Username { get; set; } = default!;

  [Required]
  [StringLength(50)]
  public string Email { get; set; } = default!;

  [Required]
  [JsonIgnore]
  public string PasswordHash { get; set; } = default!;

  [Required]
  [ForeignKey("ModuleId")]
  public int ModuleId { get; set; }

  public Module Module { get; set; } = default!;

  [Required]
  [StringLength(100)]
  public string Status { get; set; } = default!;

  [StringLength(300)]
  public string Bio { get; set; } = default!;

  public bool IsAdmin { get; set; } = default!;

  [JsonIgnore]
  public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

  [JsonIgnore]
  public ICollection<UserFollower> Followers { get; set; } = new HashSet<UserFollower>();

  [JsonIgnore]
  public ICollection<UserFollower> Following { get; set; } = new HashSet<UserFollower>();
}
