using DirectoryService.Domain.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");
        
        builder.HasKey(location => location.Id);
        
        builder
            .Property(location => location.Id)
            .HasColumnName("id");

        builder.OwnsOne(location => location.Name, nameBuilder =>
        {
            nameBuilder
                .Property(name => name.Value)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();
        });

        builder.OwnsOne(location => location.Address, addressBuilder =>
        {
            addressBuilder
                .Property(address => address.Value)
                .HasColumnName("address")
                .IsRequired();
        });

        builder
            .Property(location => location.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder
            .Property(location => location.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
    }
}