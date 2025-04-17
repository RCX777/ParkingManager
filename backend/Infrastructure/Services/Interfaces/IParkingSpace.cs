using ParkingManager.Core.DataTransferObjects;
using ParkingManager.Core.Requests;
using ParkingManager.Core.Responses;

namespace ParkingManager.Infrastructure.Services.Interfaces;
/// <summary>
/// Interface for parking complex service.
/// </summary>
public interface IParkingSpaceService
{
    Task<ServiceResponse<PagedResponse<ParkingSpaceDTO>>> GetParkingSpaces(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    Task<ServiceResponse<ParkingSpaceDTO>> GetParkingSpace(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse<ParkingSpaceDTO>> AddParkingSpace(ParkingSpaceAddDTO parkingSpace, CancellationToken cancellationToken = default);
    Task<ServiceResponse<ParkingSpaceDTO>> UpdateParkingSpace(ParkingSpaceUpdateDTO parkingSpace, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteParkingSpace(Guid id, CancellationToken cancellationToken = default);
}
