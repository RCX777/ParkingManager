namespace ParkingManager.Core.DataTransferObjects;

public class ParkingSpaceUpdateDTO
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public Guid? OwnershipDocumentId { get; set; }
}

