using ParkingManager.Core.DataTransferObjects;
using ParkingManager.Core.Requests;
using ParkingManager.Core.Responses;

namespace ParkingManager.Infrastructure.Services.Interfaces;
/// <summary>
/// Interface for parking complex service.
/// </summary>
public interface IParkingComplexService
{
    Task<ServiceResponse<PagedResponse<ParkingComplexDTO>>> GetParkingComplexes(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    Task<ServiceResponse<ParkingComplexDTO>> GetParkingComplex(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse<ParkingComplexDTO>> AddParkingComplex(ParkingComplexAddDTO parkingComplex, CancellationToken cancellationToken = default);
    Task<ServiceResponse<ParkingComplexDTO>> UpdateParkingComplex(ParkingComplexUpdateDTO parkingComplex, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteParkingComplex(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse<ParkingComplexDTO>> AddAdmin(Guid id, Guid adminId, CancellationToken cancellationToken = default);
}
