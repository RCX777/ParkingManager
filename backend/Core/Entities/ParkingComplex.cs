namespace ParkingManager.Core.Entities;

public class ParkingComplex : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<User> Admins { get; set; } = null!;

    public ICollection<ParkingSpace> ParkingSpaces { get; set; } = null!;
}

