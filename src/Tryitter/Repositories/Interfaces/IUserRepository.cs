using Tryitter.Models;
using Tryitter.Dtos.User;

namespace Tryitter.Repositories;

public interface IUserRepository : IRepository<User, CreateUserDto, UpdateUserDto, FindManyUsersDto, int>
{
  Task<User> FindByUsername(string username);
}
