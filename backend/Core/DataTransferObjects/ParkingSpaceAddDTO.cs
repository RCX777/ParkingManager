namespace ParkingManager.Core.DataTransferObjects;

public class ParkingSpaceAddDTO
{
    public Guid ParkingComplexId { get; set; }
    public Guid? UserId { get; set; }
}

