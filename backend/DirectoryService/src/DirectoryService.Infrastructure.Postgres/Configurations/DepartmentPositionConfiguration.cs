using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class DepartmentPositionConfiguration : IEntityTypeConfiguration<DepartmentPosition>
{
    public void Configure(EntityTypeBuilder<DepartmentPosition> builder)
    {
        builder.ToTable("department_positions");

        builder.HasKey(departmentPosition => departmentPosition.Id);

        builder
            .Property(departmentPosition => departmentPosition.Id)
            .HasColumnName("id");
        
        builder
            .Property(departmentPosition => departmentPosition.DepartmentId)
            .HasColumnName("department_id")
            .IsRequired();
        
        builder
            .Property(departmentPosition => departmentPosition.PositionId)
            .HasColumnName("position_id")
            .IsRequired();
        
        builder
            .HasOne<Department>()
            .WithMany()
            .HasForeignKey(department => department.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne<Position>()
            .WithMany()
            .HasForeignKey(departmentPosition => departmentPosition.PositionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(departmentPosition => new
        {
            departmentPosition.DepartmentId,
            departmentPosition.PositionId
        }).IsUnique();
    }
}