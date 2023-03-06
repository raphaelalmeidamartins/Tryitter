using Microsoft.EntityFrameworkCore;
using Tryitter.Models;
using Tryitter.Utils;

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

  public async Task SeedAsync()
  {
    using var transaction = await Database.BeginTransactionAsync();

    try
    {
      if (!Modules.Any())
      {
        Module fundamentos = new() { Name = "Fundamentos" };
        Module frontEnd = new() { Name = "FrontEnd" };
        Module BackEnd = new() { Name = "BackEnd" };
        Module computerScience = new() { Name = "Ciência da Computação" };
        Module beyond = new() { Name = "Beyond" };

        Modules.AddRange(fundamentos, frontEnd, BackEnd, computerScience, beyond);
        await SaveChangesAsync();
      }

      if (!Users.Any())
      {
        var adminModule = await Modules.SingleAsync(m => m.Name == "Beyond");

        User admin = new()
        {
          Username = "admin",
          Email = "admin@admin.com",
          PasswordHash = await PasswordHasher.HashPasswordAsync("admin"),
          Bio = "The boss",
          Status = "Bossing around",
          ModuleId = adminModule.ModuleId,
          Module = adminModule,
          IsAdmin = true,
        };

        Users.Add(admin);
        await SaveChangesAsync();
      }

      await transaction.CommitAsync();
    }
    catch (Exception ex)
    {
      await transaction.RollbackAsync();
      throw new Exception("Error seeding the database", ex);
    }
  }

}
