namespace ParkingManager.Core.Entities;

public class OwnershipDocument : BaseEntity
{
    public ParkingSpace? ParkingSpace { get; set; } = null!;

    public Guid? UserFileId { get; set; }
    public UserFile? UserFile { get; set; } = null!;
}
