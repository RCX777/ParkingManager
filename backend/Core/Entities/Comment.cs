namespace ParkingManager.Core.Entities;

public class Comment : BaseEntity
{
    public string Text { get; set; } = null!;

    public Guid ParkingSpaceId { get; set; }
    public ParkingSpace ParkingSpace { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
