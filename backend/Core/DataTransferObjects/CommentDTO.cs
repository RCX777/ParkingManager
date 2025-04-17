namespace ParkingManager.Core.DataTransferObjects;

public class CommentDTO
{
    public Guid Id { get; set; }
    public string Text { get; set; } = null!;

    public Guid ParkingSpaceId { get; set; }
    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }
}
