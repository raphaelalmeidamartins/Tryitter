using Microsoft.AspNetCore.Mvc;
using Tryitter.Repositories;
using Tryitter.Dtos.UserFollower;
using Microsoft.AspNetCore.Authorization;

namespace Tryitter.Controllers;

[ApiController]
[Route("Follow")]
public class UserFollowerController : ControllerBase
{
  private readonly ILogger<UserFollowerController> _logger;
  private readonly IUserFollowerRepository _repository;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public UserFollowerController(ILogger<UserFollowerController> logger, IUserFollowerRepository repository, IHttpContextAccessor httpContextAccessor)
  {
    this._logger = logger;
    this._repository = repository;
    this._httpContextAccessor = httpContextAccessor;
  }

  [HttpPost("{id}")]
  [Authorize]
  public async Task<IActionResult> Follow(int id)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      var userId = int.Parse(this._httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value!);
      var dto = new CreateUserFollowerDto() { FolloweeId = id, FollowerId = userId };
      await this._repository.Create(dto);

      return NoContent();
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error following user");
      if (ex is ArgumentException) return Conflict(ex.Message);
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }

  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> Unfollow(int id)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      var userId = int.Parse(this._httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value!);
      var dto = new UserFollowerIdDto() { FolloweeId = id, FollowerId = userId };
      await this._repository.Destroy(dto);

      return NoContent();
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error unfollowing user");
      if (ex is ArgumentException) return NotFound(ex.Message);
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }
}
