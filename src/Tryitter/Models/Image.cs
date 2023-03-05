using System.ComponentModel.DataAnnotations;

namespace Tryitter.Models;

public class Image
{
  public int ImageId { get; private set; }

  [Required]
  [MaxLength(255)]
  public string FileName { get; set; } = default!;

  [Required]
  [MaxLength(100)]
  public string ContentType { get; set; } = default!;

  [Required]
  [MaxLength(1048576)]
  public byte[] Data { get; set; } = default!;

  public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
}
