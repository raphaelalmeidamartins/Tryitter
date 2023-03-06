namespace Tryitter.Repositories;

public interface IBaseRepository<Entity, CreateDto, UpdateDto, FindManyDto, Id> where Entity : class
{
  Task<Entity> Create(CreateDto dto);
  Task<Entity> Destroy(Id id);
}

public interface IRepository<Entity, CreateDto, UpdateDto, FindManyDto, Id> : IBaseRepository<Entity, CreateDto, UpdateDto, FindManyDto, Id> where Entity : class
{
  Task<List<Entity>> FindMany(FindManyDto? dto);
  Task<Entity> FindById(Id id);
  Task<Entity> Update(Id id, UpdateDto dto);
}
