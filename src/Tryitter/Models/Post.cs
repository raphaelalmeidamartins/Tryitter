using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tryitter.Models;

public class Post
{
  public int PostId { get; set; }

  [Required]
  [StringLength(300)]
  public string Content { get; set; } = default!;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  [ForeignKey("AuthorId")]
  public int AuthorId { get; set; }
  public User Author { get; set; } = default!;

  public ICollection<Image> Images { get; set; } = new List<Image>();
}
