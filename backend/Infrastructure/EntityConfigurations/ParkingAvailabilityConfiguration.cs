using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ParkingManager.Core.Entities;

namespace ParkingManager.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the ParkingAvailability entity.
/// </summary>
public class ParkingAvailabilityConfiguration : IEntityTypeConfiguration<ParkingAvailability>
{
    public void Configure(EntityTypeBuilder<ParkingAvailability> builder)
    {
        builder.HasKey(pa => pa.Id); // Here it is specified that the property Id is the primary key.

        builder.Property(pa => pa.StartDate)
            .IsRequired(false);

        builder.Property(pa => pa.EndDate)
            .IsRequired(false);

        builder.HasOne(pa => pa.ParkingSpace) // This specifies a one-to-many relation.
            .WithMany(ps => ps.Availabilities) // This provides the reverse mapping for the one-to-many relation.
            .HasForeignKey(pa => pa.ParkingSpaceId) // Here the foreign key column is specified.
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // This specifies the delete behavior when the referenced entity is removed.
    }
}

