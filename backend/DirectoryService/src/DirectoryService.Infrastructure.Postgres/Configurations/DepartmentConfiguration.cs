using DirectoryService.Domain.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");

        builder.HasKey(department => department.Id);

        builder
            .Property(department => department.Id)
            .HasColumnName("id");

        builder.OwnsOne(department => department.Name, departmentBuilder =>
        {
            departmentBuilder
                .Property(name => name.Value)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();
        });

        builder.OwnsOne(department => department.Slug, departmentBuilder =>
        {
            departmentBuilder
                .Property(slug => slug.Value)
                .HasColumnName("slug")
                .HasMaxLength(100)
                .IsRequired();
            
            departmentBuilder
                .HasIndex(slug => slug.Value)
                .IsUnique();
        });

        builder.OwnsOne(department => department.Path, departmentBuilder =>
        {
            departmentBuilder
                .Property(path => path.Value)
                .HasColumnName("path")
                .HasMaxLength(1000)
                .IsRequired();
        });

        builder
            .Property(department => department.ParentId)
            .HasColumnName("parent_id");
        
        builder.HasOne<Department>()
            .WithMany()
            .HasForeignKey(department => department.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .Property(department => department.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder
            .Property(department => department.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
    }
}