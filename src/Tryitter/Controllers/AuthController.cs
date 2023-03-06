using Microsoft.AspNetCore.Mvc;
using Tryitter.Repositories;
using Tryitter.Models;
using Tryitter.Dtos.Auth;
using Tryitter.Utils;

namespace Tryitter.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
  private readonly ILogger<AuthController> _logger;
  private readonly IUserRepository _repository;
  private readonly ITokenGenerator _tokenGenerator;

  public AuthController(ILogger<AuthController> logger, IUserRepository repository, ITokenGenerator tokenGenerator)
  {
    this._logger = logger;
    this._repository = repository;
    this._tokenGenerator = tokenGenerator;
  }

  [HttpPost]
  public async Task<IActionResult> Login(AuthDto dto)
  {
    var user = await this._repository.FindByUsername(dto.Username);
    if (user == null) return Unauthorized("Incorrect username or password");

    var isPasswordCorrect = await PasswordHasher.VerifyPasswordAsync(dto.Password, user!.PasswordHash);
    if (isPasswordCorrect == false) return Unauthorized("Incorrect username or password");

    var token = this._tokenGenerator.Generate(user);
    return Ok(new { token });
  }
}
