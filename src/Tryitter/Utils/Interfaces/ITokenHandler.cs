using Tryitter.Models;

namespace Tryitter.Utils;

public interface ITokenGenerator
{
  string Generate(User user);
}
