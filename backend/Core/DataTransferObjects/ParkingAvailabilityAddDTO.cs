namespace ParkingManager.Core.DataTransferObjects;

public class ParkingAvailabilityAddDTO
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public Guid ParkingSpaceId { get; set; }
}
