using System.ComponentModel.DataAnnotations;

namespace Tryitter.Dtos.Module
{
  public class CreateModuleDto
  {
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = default!;
  }

  public class UpdateModuleDto
  {
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = default!;
  }

  public class FindManyModulesDto
  {
  }
}
