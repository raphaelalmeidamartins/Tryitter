public static class PasswordHasher
{
  public static async Task<string> HashPasswordAsync(string password)
  {
    return await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));
  }

  public static async Task<bool> VerifyPasswordAsync(string password, string hashedPassword)
  {
    return await Task.Run(() => BCrypt.Net.BCrypt.Verify(password, hashedPassword));
  }
}
