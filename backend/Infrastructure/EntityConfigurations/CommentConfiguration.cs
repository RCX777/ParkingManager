using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingManager.Core.Entities;

namespace ParkingManager.Infrastructure.EntityConfigurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(e => e.Id) // This specifies which property is configured.
            .IsRequired(); // Here it is specified if the property is required, meaning it cannot be null in the database.
        builder.HasKey(x => x.Id); // Here it is specified that the property Id is the primary key.
        builder.Property(e => e.Text)
            .HasMaxLength(255) // This specifies the maximum length for varchar type in the database.
            .IsRequired();

        builder.HasOne(e => e.ParkingSpace)
          .WithMany(e => e.Comments) // This provides the reverse mapping for the one-to-many relation.
          .HasForeignKey(e => e.ParkingSpaceId) // Here the foreign key column is specified.
          .HasPrincipalKey(e => e.Id)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade); // This specifies the delete behavior when the referenced entity is removed.

        builder.HasOne(e => e.User)
          .WithMany(e => e.Comments) // This provides the reverse mapping for the one-to-many relation.
          .HasForeignKey(e => e.UserId) // Here the foreign key column is specified.
          .HasPrincipalKey(e => e.Id)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade); // This specifies the delete behavior when the referenced entity is removed.
    }
}
