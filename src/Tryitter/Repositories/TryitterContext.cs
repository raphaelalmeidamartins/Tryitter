using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tryitter.Models;

namespace Tryitter.Model
{
  public partial class TryitterContext : DbContext
  {
    private readonly IConfiguration _config;

    public DbSet<Image> Images { get; set; } = default!;

    public TryitterContext(IConfiguration config)
    {
      _config = config;
    }

    public TryitterContext(DbContextOptions<TryitterContext> options, IConfiguration config)
        : base(options)
    {
      _config = config;
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
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
  }
}
