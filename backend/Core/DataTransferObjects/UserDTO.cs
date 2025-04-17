using ParkingManager.Core.Enums;

namespace ParkingManager.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to transfer information about a user within the application and to client application.
/// Note that it doesn't contain a password property and that is why you should use DTO rather than entities to use only the data that you need or protect sensible information.
/// </summary>
public class UserDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public UserRoleEnum Role { get; set; }

    public ICollection<ParkingSpaceDTO> ParkingSpaces { get; set; } = null!;
    public ICollection<CommentDTO> Comments { get; set; } = null!;
    public ICollection<ParkingComplexDTO> ParkingComplexes { get; set; } = null!;
}
