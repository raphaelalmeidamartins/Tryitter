using Tryitter.Models;
using Tryitter.Dtos.UserFollower;

namespace Tryitter.Repositories;

public interface IUserFollowerRepository : IBaseRepository<UserFollower, CreateUserFollowerDto, UpdateUserFollowerDto, FindManyUserFollowersDto, UserFollowerIdDto>
{
}
