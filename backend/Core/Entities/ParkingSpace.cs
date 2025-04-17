namespace ParkingManager.Core.Entities;

public class ParkingSpace : BaseEntity
{
    public User? User { get; set; } = null!;
    public Guid? UserId { get; set; }

    public Guid ParkingComplexId { get; set; }
    public ParkingComplex ParkingComplex { get; set; } = null!;

    public Guid OwnershipDocumentId { get; set; }
    public OwnershipDocument OwnershipDocument { get; set; } = null!;

    public ICollection<ParkingAvailability> Availabilities { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = null!;
}
