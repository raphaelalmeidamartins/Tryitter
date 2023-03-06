using Tryitter.Models;
using Tryitter.Dtos.Module;

namespace Tryitter.Repositories;

public interface IModuleRepository : IRepository<Module, CreateModuleDto, UpdateModuleDto, FindManyModulesDto, int>
{
}
