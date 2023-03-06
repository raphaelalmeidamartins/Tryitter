using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tryitter.Models;

public class Module
{
  public int ModuleId { get; private set; }

  [Required]
  [MaxLength(255)]
  public string Name { get; set; } = default!;

  [JsonIgnore]
  public ICollection<User> Users { get; set; } = new HashSet<User>();
}