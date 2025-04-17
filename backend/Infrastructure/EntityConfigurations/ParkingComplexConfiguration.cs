using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingManager.Core.Entities;

namespace ParkingManager.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the ParkingComplex entity.
/// </summary>
public class ParkingComplexConfiguration : IEntityTypeConfiguration<ParkingComplex>
{
    public void Configure(EntityTypeBuilder<ParkingComplex> builder)
    {
        builder.HasKey(x => x.Id); // Here it is specified that the property Id is the primary key.

        builder.Property(e => e.Name)
            .HasMaxLength(255) // This specifies the maximum length for varchar type in the database.
            .IsRequired();

        builder.Property(e => e.Address)
            .HasMaxLength(255) // This specifies the maximum length for varchar type in the database.
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(4095) // This specifies the maximum length for varchar type in the database.
            .IsRequired(false); // This specifies that this column can be null in the database.

        builder.HasMany(e => e.ParkingSpaces) // This specifies a one-to-many relation.
            .WithOne(e => e.ParkingComplex) // This provides the reverse mapping for the one-to-many relation.
            .HasForeignKey(e => e.ParkingComplexId) // Here the foreign key column is specified.
            .HasPrincipalKey(e => e.Id) // This specifies the referenced key in the referenced table.
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Admins) // This specifies a many-to-many relation.
          .WithMany(e => e.ParkingComplexes) // This provides the reverse mapping for the many-to-many relation.
          .UsingEntity(j => j.ToTable("ParkingComplexAdmins")); // This specifies the join table name.

    }
}
