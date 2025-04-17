namespace ParkingManager.Core.DataTransferObjects;

public class OwnershipDocumentDTO
{
    public Guid Id { get; set; }
    public ParkingSpaceDTO? ParkingSpaceDTO { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? UserFileId { get; set; }
}

