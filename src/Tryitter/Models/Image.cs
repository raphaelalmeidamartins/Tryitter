namespace Tryitter.Models;

public class Image
{
  public int ImageId { get; private set; }
  public string FileName { get; set; } = default!;
  public string ContentType { get; set; } = default!;
  public byte[] Data { get; set; } = default!;
}
