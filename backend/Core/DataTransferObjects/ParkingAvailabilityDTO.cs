namespace ParkingManager.Core.DataTransferObjects;

public class ParkingAvailabilityDTO
{
    public Guid Id { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public Guid ParkingSpaceId { get; set; }
}
