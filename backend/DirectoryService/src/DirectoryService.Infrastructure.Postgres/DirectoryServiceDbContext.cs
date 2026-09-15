using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Positions;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.Postgres;

public class DirectoryServiceDbContext : DbContext
{
    public DirectoryServiceDbContext(DbContextOptions<DirectoryServiceDbContext> options) : base(options)
    {
    }
    
    public DbSet<Department> Departments { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<DepartmentLocation> DepartmentLocations { get; set; }
    public DbSet<DepartmentPosition> DepartmentPositions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DirectoryServiceDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}