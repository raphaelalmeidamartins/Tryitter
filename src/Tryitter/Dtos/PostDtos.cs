using System.ComponentModel.DataAnnotations;

namespace Tryitter.Dtos.Post
{
  public class CreatePostDto
  {
    [Required]
    public int AuthorId { get; set; }

    [Required]
    [StringLength(300)]
    public string Content { get; set; } = default!;

    public IFormFile? Image { get; set; } = default!;
  }

  public class UpdatePostDto
  {
    [Required]
    [StringLength(300)]
    public string Content { get; set; } = default!;
  }

  public class FindManyPostsDto
  {
    public int? AuthorId { get; set; } 
  }
}
