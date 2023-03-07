using Tryitter.Models;
using Microsoft.EntityFrameworkCore;
using Tryitter.Dtos.User;
using Tryitter.Utils;

namespace Tryitter.Repositories;

public class UserRepository : IUserRepository
{
  private readonly TryitterContext _context;

  public UserRepository(TryitterContext context)
  {
    this._context = context;
  }

  public async Task<User> Create(CreateUserDto dto)
  {
    var existingUsername = await this._context.Users
        .FirstOrDefaultAsync(u => u.Username == dto.Username);

    if (existingUsername != null)
    {
      throw new ArgumentException($"Username {dto.Username} already in use");
    }

    var existingEmail = await this._context.Users
        .FirstOrDefaultAsync(u => u.Email == dto.Email);

    if (existingEmail != null)
    {
      throw new ArgumentException($"Email {dto.Email} already in use");
    }

    var existingModule = await this._context.Modules.FindAsync(dto.ModuleId);
    if (existingModule == null)
    {
      throw new ArgumentException($"User was not created because Module with id {dto.ModuleId} was not found");
    }

    var user = new User()
    {
      Username = dto.Username,
      Email = dto.Email,
      PasswordHash = await PasswordHasher.HashPasswordAsync(dto.Password),
      ModuleId = dto.ModuleId,
      Status = dto.Status,
      Bio = dto.Bio,
      IsAdmin = false,
    };

    await this._context.Users.AddAsync(user);
    await this._context.SaveChangesAsync();

    return user;
  }


  public async Task<List<User>> FindMany(FindManyUsersDto? dto)
  {
    IQueryable<User> query = this._context.Users;

    if (dto?.ModuleId != null)
    {
      query = query.Where(u => u.ModuleId == dto.ModuleId);
    }

    var users = await query
      .ToListAsync();

    return users;
  }

  public async Task<User> FindById(int id)
  {
    var entry = await this._context.Users
      .FirstOrDefaultAsync(u => u.UserId == id);

    if (entry == null)
    {
      throw new ArgumentException($"User with Id {id} not found");
    }

    return entry!;
  }

  public async Task<User> FindByUsername(string username)
  {
    var entry = await this._context.Users
      .FirstOrDefaultAsync(u => u.Username == username);

    if (entry == null)
    {
      throw new ArgumentException($"User with the username {username} not found");
    }

    return entry!;
  }

  public async Task<User> Update(int id, UpdateUserDto dto)
  {
    var user = await this.FindById(id);

    var existingModule = await this._context.Modules.FindAsync(dto.ModuleId);
    if (existingModule == null)
    {
      throw new ArgumentException($"User was not updated because Module with id {dto.ModuleId} was not found");
    }

    if (dto.ModuleId != null) user.ModuleId = dto.ModuleId.Value;
    if (dto.Status != null) user.Status = dto.Status;
    if (dto.Bio != null) user.Bio = dto.Bio;

    if (dto.Password != null)
    {
      user.PasswordHash = await PasswordHasher.HashPasswordAsync(dto.Password);
    }

    await this._context.SaveChangesAsync();

    return user;
  }

  public async Task<User> Destroy(int id)
  {
    var user = await this.FindById(id);
    this._context.Users.Remove(user);

    await this._context.SaveChangesAsync();

    return user;
  }
}
