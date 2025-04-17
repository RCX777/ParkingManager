using ParkingManager.Core.DataTransferObjects;
using ParkingManager.Core.Entities;
using ParkingManager.Core.Errors;
using ParkingManager.Core.Requests;
using ParkingManager.Core.Responses;
using ParkingManager.Core.Specifications;
using ParkingManager.Infrastructure.Database;
using ParkingManager.Infrastructure.Repositories.Interfaces;
using ParkingManager.Infrastructure.Services.Interfaces;

namespace ParkingManager.Infrastructure.Services.Implementations;

public class ParkingAvailabilityService(IRepository<WebAppDatabaseContext> repository) : IParkingAvailabilityService
{
    public async Task<ServiceResponse<PagedResponse<ParkingAvailabilityDTO>>> GetParkingAvailabilitys(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new ParkingAvailabilityProjectionSpec(true), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse<ParkingAvailabilityDTO>> GetParkingAvailability(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new ParkingAvailabilityProjectionSpec(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<ParkingAvailabilityDTO>(CommonErrors.ParkingAvailabilityNotFound);
    }
    public async Task<ServiceResponse<ParkingAvailabilityDTO>> AddParkingAvailability(ParkingAvailabilityAddDTO parkingAvailability, CancellationToken cancellationToken = default)
    {
        var entity = new ParkingAvailability
        {
            StartDate = parkingAvailability.StartDate,
            EndDate = parkingAvailability.EndDate,
            ParkingSpaceId = parkingAvailability.ParkingSpaceId,
        };

        var result = await repository.AddAsync(entity, cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError<ParkingAvailabilityDTO>(CommonErrors.TechnicalSupport);
        }

        var parkingAvailabilityDto = new ParkingAvailabilityDTO
        {
            Id = result.Id,
            StartDate = result.StartDate,
            EndDate = result.EndDate,
            ParkingSpaceId = result.ParkingSpaceId,
        };

        return ServiceResponse.ForSuccess(parkingAvailabilityDto);
    }
    public async Task<ServiceResponse<ParkingAvailabilityDTO>> UpdateParkingAvailability(ParkingAvailabilityUpdateDTO parkingAvailability, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(new ParkingAvailabilitySpec(parkingAvailability.Id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError<ParkingAvailabilityDTO>(CommonErrors.ParkingAvailabilityNotFound);
        }

        entity.StartDate = parkingAvailability.StartDate ?? entity.StartDate;
        entity.EndDate = parkingAvailability.EndDate ?? entity.EndDate;

        var result = await repository.UpdateAsync(entity, cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError<ParkingAvailabilityDTO>(CommonErrors.TechnicalSupport);
        }

        var parkingAvailabilityDto = new ParkingAvailabilityDTO
        {
            Id = result.Id,
            StartDate = result.StartDate,
            EndDate = result.EndDate,
            ParkingSpaceId = result.ParkingSpaceId,
        };

        return ServiceResponse.ForSuccess(parkingAvailabilityDto);
    }
    public async Task<ServiceResponse> DeleteParkingAvailability(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(new ParkingAvailabilitySpec(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.ParkingAvailabilityNotFound);
        }

        await repository.DeleteAsync<ParkingAvailability>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
