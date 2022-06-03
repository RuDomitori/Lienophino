using CookSolver.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CookSolver.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Meal>();

        modelBuilder.Entity<MealHistoryItem>()
            .HasKey(x => new {x.MealId, x.Date});

        base.OnModelCreating(modelBuilder);
    }
}