using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tryitter.Models;

public class Post
{
  public int PostId { get; private set; }

  [Required]
  [StringLength(300)]
  public string Content { get; set; } = default!;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  [ForeignKey("AuthorId")]
  public int AuthorId { get; set; }
  public User Author { get; set; } = default!;

  [ForeignKey("ImageId")]
  public int? ImageId { get; set; }

  public Image? Image { get; set; } = default!;
}
