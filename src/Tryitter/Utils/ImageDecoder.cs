namespace Tryitter.Utils;

public class ImageDecoder
{
  public static async Task<byte[]> GetFileData(IFormFile file)
  {
    await using var memoryStream = new MemoryStream();
    await file.CopyToAsync(memoryStream);
    return memoryStream.ToArray();
  }
}
