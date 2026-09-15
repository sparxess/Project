using DirectoryService.Domain.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("positions");
        
        builder.HasKey(position => position.Id);
        
        builder
            .Property(position => position.Id)
            .HasColumnName("id");

        builder.OwnsOne(position => position.Name, positionBuilder =>
        {
            positionBuilder
                .Property(name => name.Value)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();
        });
        
        builder
            .Property(position => position.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        builder
            .Property(position => position.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
    }
}