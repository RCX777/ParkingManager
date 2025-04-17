using ParkingManager.Core.DataTransferObjects;
using ParkingManager.Core.Requests;
using ParkingManager.Core.Responses;

namespace ParkingManager.Infrastructure.Services.Interfaces;
/// <summary>
/// Interface for parking complex service.
/// </summary>
public interface IParkingAvailabilityService
{
    Task<ServiceResponse<PagedResponse<ParkingAvailabilityDTO>>> GetParkingAvailabilitys(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    Task<ServiceResponse<ParkingAvailabilityDTO>> GetParkingAvailability(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse<ParkingAvailabilityDTO>> AddParkingAvailability(ParkingAvailabilityAddDTO parkingAvailability, CancellationToken cancellationToken = default);
    Task<ServiceResponse<ParkingAvailabilityDTO>> UpdateParkingAvailability(ParkingAvailabilityUpdateDTO parkingAvailability, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteParkingAvailability(Guid id, CancellationToken cancellationToken = default);
}
