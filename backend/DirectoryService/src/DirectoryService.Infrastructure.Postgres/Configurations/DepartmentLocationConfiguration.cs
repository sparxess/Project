using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class DepartmentLocationConfiguration : IEntityTypeConfiguration<DepartmentLocation>
{
    public void Configure(EntityTypeBuilder<DepartmentLocation> builder)
    {
        builder.ToTable("department_locations");

        builder.HasKey(departmentLocation => departmentLocation.Id);

        builder
            .Property(departmentLocation => departmentLocation.Id)
            .HasColumnName("id");

        builder
            .Property(departmentLocation => departmentLocation.IsPrimary)
            .HasColumnName("is_primary")
            .IsRequired();
        
        builder
            .Property(departmentLocation => departmentLocation.DepartmentId)
            .HasColumnName("department_id")
            .IsRequired();

        builder
            .Property(departmentLocation => departmentLocation.LocationId)
            .HasColumnName("location_id")
            .IsRequired();
        
        builder.HasOne<Department>()
            .WithMany()
            .HasForeignKey(departmentLocation => departmentLocation.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<Location>()
            .WithMany()
            .HasForeignKey(departmentLocation => departmentLocation.LocationId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(departmentLocation => new
        {
            departmentLocation.DepartmentId,
            departmentLocation.LocationId
        }).IsUnique();
    }
}