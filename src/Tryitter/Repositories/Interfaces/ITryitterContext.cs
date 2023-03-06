using Microsoft.EntityFrameworkCore;
using Tryitter.Models;

namespace Tryitter.Repositories;

public interface ITryitterContext
{
  public DbSet<Image> Images { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<UserFollower> UserFollowers { get; set; }
  public DbSet<Module> Modules { get; set; }
  public DbSet<Post> Posts { get; set; }
  public int SaveChanges();
}
