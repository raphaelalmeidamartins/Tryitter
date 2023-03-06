using Tryitter.Models;
using Microsoft.EntityFrameworkCore;
using Tryitter.Dtos.Module;

namespace Tryitter.Repositories;

public class ModuleRepository : IModuleRepository
{
  private readonly TryitterContext _context;

  public ModuleRepository(TryitterContext context)
  {
    this._context = context;
  }

  public async Task<Module> Create(CreateModuleDto dto)
  {
    var module = new Module()
    {
      Name = dto.Name,
    };

    await this._context.Modules.AddAsync(module);
    await this._context.SaveChangesAsync();
    await this._context.Entry(module).ReloadAsync();
    return module;
  }

  public async Task<List<Module>> FindMany(FindManyModulesDto? dto)
  {
    var modules = await this._context.Modules
      .ToListAsync();

    return modules!;
  }

  public async Task<Module> FindById(int id)
  {
    var entry = await this._context.Modules
      .FirstOrDefaultAsync(m => m.ModuleId == id);

    if (entry == null)
    {
      throw new ArgumentException($"Module with Id {id} not found");
    }

    return entry!;
  }

  public async Task<Module> Update(int id, UpdateModuleDto dto)
  {
    var module = await this.FindById(id);

    module.Name = dto.Name;

    await this._context.SaveChangesAsync();

    return module;
  }

  public async Task<Module> Destroy(int id)
  {
    var module = await this.FindById(id);
    this._context.Modules.Remove(module);

    await this._context.SaveChangesAsync();

    return module;
  }
}
