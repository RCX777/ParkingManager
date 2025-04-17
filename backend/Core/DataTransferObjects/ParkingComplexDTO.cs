namespace ParkingManager.Core.DataTransferObjects;

public class ParkingComplexDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<UserDTO> Admins { get; set; } = null!;

    public ICollection<ParkingSpaceDTO> ParkingSpaces { get; set; } = null!;
};
