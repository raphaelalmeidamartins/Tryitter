using System.ComponentModel.DataAnnotations;

namespace Tryitter.Dtos.User
{
  public class CreateUserDto
  {
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = default!;

    [Required]
    [StringLength(50)]
    public string Email { get; set; } = default!;

    [Required]
    [StringLength(50)]
    public string Password { get; set; } = default!;

    [Required]
    public int ModuleId { get; set; }

    [Required]
    [StringLength(100)]
    public string Status { get; set; } = default!;

    [StringLength(300)]
    public string Bio { get; set; } = default!;

    public IFormFile? ProfilePicture { get; set; } = default!;
  }
  public class UpdateUserDto
  {
    public string? Password { get; set; }

    public int? ModuleId { get; set; }

    [StringLength(100)]
    public string? Status { get; set; }

    [StringLength(300)]
    public string? Bio { get; set; }

    public IFormFile? ProfilePicture { get; set; } = default!;
  }

  public class FindManyUsersDto
  {
    public int? ModuleId { get; set; }
  }
}
