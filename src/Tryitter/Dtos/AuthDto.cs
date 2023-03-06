using System.ComponentModel.DataAnnotations;

namespace Tryitter.Dtos.Auth
{
  public class AuthDto
  {
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = default!;

    [Required]
    [StringLength(50)]
    public string Password { get; set; } = default!;
  }
}
