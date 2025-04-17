namespace ParkingManager.Core.Entities;

public class ParkingAvailability : BaseEntity
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public Guid ParkingSpaceId { get; set; }
    public ParkingSpace ParkingSpace { get; set; } = null!;
}
