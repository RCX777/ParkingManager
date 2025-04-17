namespace ParkingManager.Core.DataTransferObjects;

public class ParkingComplexAddDTO
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? Description { get; set; }
};
