using Microsoft.AspNetCore.Mvc;
using Tryitter.Repositories;
using Tryitter.Dtos.User;
using Microsoft.AspNetCore.Authorization;

namespace Tryitter.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
  private readonly ILogger<UserController> _logger;
  private readonly IUserRepository _repository;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public UserController(ILogger<UserController> logger, IUserRepository repository, IHttpContextAccessor httpContextAccessor)
  {
    this._logger = logger;
    this._repository = repository;
    this._httpContextAccessor = httpContextAccessor;
  }

  [HttpPost]
  public async Task<IActionResult> Create(CreateUserDto dto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      var user = await this._repository.Create(dto);

      return CreatedAtAction(nameof(Create), new { id = user.UserId }, user);
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error creating user");
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }

  [HttpGet]
  [Authorize]
  public async Task<IActionResult> FindAll(int? moduleId)
  {
    try
    {
      var dto = new FindManyUsersDto { ModuleId = moduleId };
      var users = await this._repository.FindMany(dto);
      return Ok(users);
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error listing the users");
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }


  [HttpGet("{id}")]
  [Authorize]
  public async Task<IActionResult> FindById(int id)
  {
    try
    {
      var user = await this._repository.FindById(id);
      return Ok(user);
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error finding the user");
      if (ex is ArgumentException) return NotFound(ex.Message);
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }

  [HttpGet("username/{username}")]
  [Authorize]
  public async Task<IActionResult> FindByUsername(string username)
  {
    try
    {
      var user = await this._repository.FindByUsername(username);
      return Ok(user);
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error finding the user");
      if (ex is ArgumentException) return NotFound(ex.Message);
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }

  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> Update(int id, UpdateUserDto dto)
  {
    var userId = int.Parse(this._httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value!);

    if (userId != id)
    {
      return Forbid();
    }

    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      var user = await this._repository.Update(id, dto);
      return NoContent();
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error updating user");
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }

  [HttpDelete("{id}")]
  [Authorize(Policy = "AdminPolicy")]
  public async Task<IActionResult> Destroy(int id)
  {
    try
    {
      var user = await this._repository.Destroy(id);
      return NoContent();
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error deleting user");
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }
}
