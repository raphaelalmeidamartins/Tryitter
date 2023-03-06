using Microsoft.AspNetCore.Mvc;
using Tryitter.Repositories;
using Tryitter.Models;
using Tryitter.Dtos.Post;
using Microsoft.AspNetCore.Authorization;

namespace Tryitter.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
  private readonly ILogger<PostController> _logger;
  private readonly IPostRepository _repository;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public PostController(ILogger<PostController> logger, IPostRepository repository, IHttpContextAccessor httpContextAccessor)
  {
    this._logger = logger;
    this._repository = repository;
    this._httpContextAccessor = httpContextAccessor;
  }

  [HttpPost]
  [Authorize]
  public async Task<IActionResult> Create(CreatePostDto dto)
  {
    var userId = int.Parse(this._httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value!);

    if (userId != dto.AuthorId)
    {
      return Forbid();
    }

    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      var post = await _repository.Create(dto);

      return CreatedAtAction(nameof(Create), new { id = post.PostId }, post);
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error creating post");
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }

  [HttpGet]
  [Authorize]
  public async Task<IActionResult> FindAll(int? authorId)
  {
    try
    {
      var dto = new FindManyPostsDto { AuthorId = authorId };
      var posts = await this._repository.FindMany(dto);
      return Ok(posts);
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error listing the posts");
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }

  [HttpGet("{id}")]
  [Authorize]
  public async Task<IActionResult> FindById(int id)
  {
    try
    {
      var post = await this._repository.FindById(id);
      return Ok(post);
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error finding the post");
      if (ex is ArgumentException) return NotFound(ex.Message);
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }

  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> Update(int id, UpdatePostDto dto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var userId = int.Parse(this._httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value!);
    var post = await this._repository.FindById(id);

    if (userId != post.AuthorId)
    {
      return Forbid();
    }

    try
    {
      await _repository.Update(id, dto);
      return NoContent();
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error updating post");
      if (ex is ArgumentException) return NotFound(ex.Message);
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }

  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> Destroy(int id)
  {
    try
    {
      var userId = int.Parse(this._httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value!);
      var post = await this._repository.FindById(id);

      if (userId != post.AuthorId)
      {
        return Forbid();
      }

      await _repository.Destroy(id);
      return NoContent();
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error deleting post");
      if (ex is ArgumentException) return NotFound(ex.Message);
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }
}
