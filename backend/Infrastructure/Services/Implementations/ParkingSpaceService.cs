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
/// <summary>
/// Interface for parking complex service.
/// </summary>
public class ParkingSpaceService(IRepository<WebAppDatabaseContext> repository) : IParkingSpaceService
{
    public async Task<ServiceResponse<PagedResponse<ParkingSpaceDTO>>> GetParkingSpaces(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new ParkingSpaceProjectionSpec(true), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse<ParkingSpaceDTO>> GetParkingSpace(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new ParkingSpaceProjectionSpec(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<ParkingSpaceDTO>(CommonErrors.ParkingSpaceNotFound);
    }
    public async Task<ServiceResponse<ParkingSpaceDTO>> AddParkingSpace(ParkingSpaceAddDTO parkingSpace, CancellationToken cancellationToken = default)
    {
        var document = new OwnershipDocument{};
        var documentResult = await repository.AddAsync(document, cancellationToken);
        if (documentResult == null)
        {
            return ServiceResponse.FromError<ParkingSpaceDTO>(CommonErrors.TechnicalSupport); // This should not happen
        }

        var entity = new ParkingSpace
        {
            ParkingComplexId = parkingSpace.ParkingComplexId,
            UserId = parkingSpace.UserId,
            OwnershipDocumentId = documentResult.Id,
            Availabilities = [],
            Comments = [],
        };

        var result = await repository.AddAsync(entity, cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError<ParkingSpaceDTO>(CommonErrors.CannotAddParkingSpace);
        }

        documentResult.ParkingSpace = entity;
        var documentUpdateResult = await repository.UpdateAsync(documentResult, cancellationToken);
        if (documentUpdateResult == null)
        {
            return ServiceResponse.FromError<ParkingSpaceDTO>(CommonErrors.TechnicalSupport); // This should not happen
        }

        var parkingSpaceDto = new ParkingSpaceDTO
        {
            Id = result.Id,
            ParkingComplexId = result.ParkingComplexId,
            UserId = result.UserId,
            OwnershipDocumentId = result.OwnershipDocumentId,
            Availabilities = [],
            Comments = [],
        };

        return ServiceResponse.ForSuccess(parkingSpaceDto);
    }
    public async Task<ServiceResponse<ParkingSpaceDTO>> UpdateParkingSpace(ParkingSpaceUpdateDTO parkingSpace, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new ParkingSpaceSpec(parkingSpace.Id), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError<ParkingSpaceDTO>(CommonErrors.ParkingSpaceNotFound);
        }
        result.UserId = parkingSpace.UserId ?? result.UserId;
        result.OwnershipDocumentId = parkingSpace.OwnershipDocumentId ?? result.OwnershipDocumentId;

        var updatedResult = await repository.UpdateAsync(result, cancellationToken);
        if (updatedResult == null)
        {
            return ServiceResponse.FromError<ParkingSpaceDTO>(CommonErrors.CannotUpdateParkingSpace);
        }
        var dto = await repository.GetAsync(new ParkingSpaceProjectionSpec(updatedResult.Id), cancellationToken);
        if (dto == null)
        {
            return ServiceResponse.FromError<ParkingSpaceDTO>(CommonErrors.TechnicalSupport); // This should not happen
        }

        return ServiceResponse.ForSuccess(dto);
    }
    public async Task<ServiceResponse> DeleteParkingSpace(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(new ParkingSpaceSpec(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.ParkingSpaceNotFound);
        }

        await repository.DeleteAsync<ParkingSpace>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
