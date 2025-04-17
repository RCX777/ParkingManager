namespace ParkingManager.Core.DataTransferObjects;

public class OwnershipDocumentAddDTO
{
    public Guid? UserFileId { get; set; }
    public ParkingSpaceDTO? ParkingSpaceDTO { get; set; }
}

