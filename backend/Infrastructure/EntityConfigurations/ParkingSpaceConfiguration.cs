using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ParkingManager.Core.Entities;

namespace ParkingManager.Infrastructure.EntityConfigurations;

public class ParkingSpaceConfiguration : IEntityTypeConfiguration<ParkingSpace>
{
    public void Configure(EntityTypeBuilder<ParkingSpace> builder)
    {
        builder.HasKey(ps => ps.Id);

        builder.HasOne(ps => ps.User)
            .WithMany(u => u.ParkingSpaces)
            .IsRequired(false)
            .HasForeignKey(ps => ps.UserId);

        builder.HasOne(ps => ps.OwnershipDocument)
            .WithOne(od => od.ParkingSpace)
            .HasForeignKey<ParkingSpace>(ps => ps.OwnershipDocumentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ps => ps.ParkingComplex)
            .WithMany(pc => pc.ParkingSpaces)
            .HasForeignKey(ps => ps.ParkingComplexId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(ps => ps.Availabilities)
            .WithOne(pa => pa.ParkingSpace)
            .HasForeignKey(pa => pa.ParkingSpaceId);

        builder.HasMany(ps => ps.Comments)
            .WithOne(c => c.ParkingSpace)
            .HasForeignKey(c => c.ParkingSpaceId);
    }
}
