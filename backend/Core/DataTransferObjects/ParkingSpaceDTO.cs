namespace ParkingManager.Core.DataTransferObjects;

public class ParkingSpaceDTO
{
    public Guid Id { get; set; }

    public Guid ParkingComplexId { get; set; }
    public Guid? UserId { get; set; }
    public Guid OwnershipDocumentId { get; set; }

    public ICollection<ParkingAvailabilityDTO> Availabilities { get; set; } = null!;
    public ICollection<CommentDTO> Comments { get; set; } = null!;
}
