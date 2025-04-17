using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ParkingManager.Core.Entities;

namespace ParkingManager.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the OwnershipDocument entity.
/// </summary>
public class OwnershipDocumentConfiguration : IEntityTypeConfiguration<OwnershipDocument>
{
    public void Configure(EntityTypeBuilder<OwnershipDocument> builder)
    {
        builder.HasKey(od => od.Id); // Here it is specified that the property Id is the primary key.

        builder.HasOne(od => od.ParkingSpace)
            .WithOne(ps => ps.OwnershipDocument)
            .HasForeignKey<ParkingSpace>(ps => ps.OwnershipDocumentId) // Here the foreign key column is specified.
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(od => od.UserFile)
            .WithOne(uf => uf.OwnershipDocument)
            .IsRequired(false)
            .HasForeignKey<OwnershipDocument>(od => od.UserFileId) // Here the foreign key column is specified.
            .OnDelete(DeleteBehavior.Cascade);
    }
}

