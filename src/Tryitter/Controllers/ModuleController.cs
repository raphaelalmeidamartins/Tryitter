using Microsoft.AspNetCore.Mvc;
using Tryitter.Repositories;
using Tryitter.Models;
using Tryitter.Dtos.Module;
using Microsoft.AspNetCore.Authorization;

namespace Tryitter.Controllers;

[ApiController]
[Route("[controller]")]
public class ModuleController : ControllerBase
{
  private readonly ILogger<ModuleController> _logger;
  private readonly IModuleRepository _repository;

  public ModuleController(ILogger<ModuleController> logger, IModuleRepository repository)
  {
    this._logger = logger;
    this._repository = repository;
  }

  [HttpPost]
  [Authorize(Policy = "AdminPolicy")]
  public async Task<IActionResult> Create(CreateModuleDto dto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      var module = await _repository.Create(dto);

      return CreatedAtAction(nameof(Create), new { id = module.ModuleId }, module);
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error creating module");
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }

  [HttpGet]
  public async Task<IActionResult> FindAll()
  {
    try
    {
      var modules = await this._repository.FindMany(null);
      return Ok(modules);
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error listing the modules");
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> FindById(int id)
  {
    try
    {
      var module = await this._repository.FindById(id);
      return Ok(module);
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error finding the module");
      if (ex is ArgumentException) return NotFound(ex.Message);
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }

  [HttpPut("{id}")]
  [Authorize(Policy = "AdminPolicy")]
  public async Task<IActionResult> Update(int id, UpdateModuleDto dto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    try
    {
      await _repository.Update(id, dto);
      return NoContent();
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error updating module");
      if (ex is ArgumentException) return NotFound(ex.Message);
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }

  [HttpDelete("{id}")]
  [Authorize(Policy = "AdminPolicy")]
  public async Task<IActionResult> Destroy(int id)
  {
    try
    {
      await _repository.Destroy(id);
      return NoContent();
    }
    catch (Exception ex)
    {
      this._logger.LogError(ex, "Error deleting module");
      if (ex is ArgumentException) return NotFound(ex.Message);
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }
}
