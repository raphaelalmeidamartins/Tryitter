using System.ComponentModel.DataAnnotations;

namespace Tryitter.Models;

public class Module
{
  public int ModuleId { get; private set; }

  [Required]
  [MaxLength(255)]
  public string Name { get; set; } = default!;

  public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
}