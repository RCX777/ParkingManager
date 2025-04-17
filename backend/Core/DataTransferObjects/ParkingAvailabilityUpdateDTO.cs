namespace ParkingManager.Core.DataTransferObjects;

public class ParkingAvailabilityUpdateDTO
{
    public Guid Id { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
