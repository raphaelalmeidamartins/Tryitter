using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

  [ForeignKey("ProfilePictureId")]
  public int ProfilePictureId { get; set; }

  public Image ProfilePicture { get; set; } = default!;

  [Required]
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
}
