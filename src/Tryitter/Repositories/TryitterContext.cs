using Microsoft.EntityFrameworkCore;
using Tryitter.Models;

namespace Tryitter.Repositories;

public partial class TryitterContext : DbContext, ITryitterContext
{
  private readonly IConfiguration _config;

  public DbSet<Image> Images { get; set; } = default!;
  public DbSet<User> Users { get; set; } = default!;
  public DbSet<UserFollower> UserFollowers { get; set; } = default!;
  public DbSet<Module> Modules { get; set; } = default!;
  public DbSet<Post> Posts { get; set; } = default!;

  public TryitterContext(IConfiguration config)
  {
    this._config = config;
  }

  public TryitterContext(DbContextOptions<TryitterContext> options, IConfiguration config)
      : base(options)
  {
    this._config = config;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
    {
      optionsBuilder.UseSqlServer(_config.GetConnectionString("Tryitter_DB"));
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    OnModelCreatingPartial(modelBuilder);

    modelBuilder.Entity<UserFollower>()
                .HasKey(uf => new { uf.FolloweeId, uf.FollowerId });

    modelBuilder.Entity<UserFollower>()
                .HasOne(uf => uf.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(uf => uf.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<UserFollower>()
                .HasOne(uf => uf.Followee)
                .WithMany(u => u.Followers)
                .HasForeignKey(uf => uf.FolloweeId)
                .OnDelete(DeleteBehavior.Cascade);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
