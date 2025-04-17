using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ParkingManager.Core.Entities;
using ParkingManager.Core.Enums;

namespace ParkingManager.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the User entity, generally the Entity Framework will figure out most of the configuration but,
/// for some specifics such as unique keys, indexes and foreign keys it is better to explicitly specify them.
/// Note that the EntityTypeBuilder implements a Fluent interface, meaning it is a highly declarative interface using method-chaining.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.Id) // This specifies which property is configured.
            .IsRequired(); // Here it is specified if the property is required, meaning it cannot be null in the database.
        builder.HasKey(x => x.Id); // Here it is specified that the property Id is the primary key.
        builder.Property(e => e.Name)
            .HasMaxLength(255) // This specifies the maximum length for varchar type in the database.
            .IsRequired();
        builder.Property(e => e.Email)
            .HasMaxLength(255)
            .IsRequired();
        builder.HasAlternateKey(e => e.Email); // Here it is specified that the property Email is a unique key.
        builder.Property(e => e.Password)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(e => e.Role)
            .HasConversion(new EnumToStringConverter<UserRoleEnum>())
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        builder.HasMany(e => e.ParkingComplexes) // This specifies a many-to-many relation.
            .WithMany(e => e.Admins) // This provides the reverse mapping for the many-to-many relation.
            .UsingEntity(j => j.ToTable("ParkingComplexAdmins")); // This specifies the join table name.

        builder.HasMany(e => e.Comments)  // This specifies a one-to-many relation
            .WithOne(e => e.User) // This provides the reverse mapping for the one-to-many relation.
            .HasForeignKey(e => e.UserId) // Here the foreign key column is specified.
            .OnDelete(DeleteBehavior.Cascade); // This specifies the delete behavior when the referenced entity is removed.

        builder.HasMany(e => e.ParkingSpaces)  // This specifies a one-to-many relation
            .WithOne(e => e.User) // This provides the reverse mapping for the one-to-many relation.
            .HasForeignKey(e => e.UserId) // Here the foreign key column is specified.
            .OnDelete(DeleteBehavior.Cascade); // This specifies the delete behavior when the referenced entity is removed.
    }
}
