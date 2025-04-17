using ParkingManager.Core.DataTransferObjects;
using ParkingManager.Core.Entities;
using ParkingManager.Core.Enums;
using ParkingManager.Core.Errors;
using ParkingManager.Core.Requests;
using ParkingManager.Core.Responses;
using ParkingManager.Core.Specifications;
using ParkingManager.Infrastructure.Database;
using ParkingManager.Infrastructure.Repositories.Interfaces;
using ParkingManager.Infrastructure.Services.Interfaces;

namespace ParkingManager.Infrastructure.Services.Implementations;

public class ParkingComplexService(IRepository<WebAppDatabaseContext> repository) : IParkingComplexService
{
    public async Task<ServiceResponse<PagedResponse<ParkingComplexDTO>>> GetParkingComplexes(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new ParkingComplexProjectionSpec(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<ParkingComplexDTO>> GetParkingComplex(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new ParkingComplexProjectionSpec(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<ParkingComplexDTO>(CommonErrors.ParkingComplexNotFound);
    }

    public async Task<ServiceResponse<ParkingComplexDTO>> AddParkingComplex(ParkingComplexAddDTO parkingComplex, CancellationToken cancellationToken = default)
    {
        var entity = new ParkingComplex
        {
            Name = parkingComplex.Name,
            Address = parkingComplex.Address,
            Description = parkingComplex.Description,
            Admins = [],
            ParkingSpaces = [],
        };

        var result = await repository.AddAsync(entity, cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError<ParkingComplexDTO>(CommonErrors.ParkingComplexNotFound);
        }

        var parkingComplexDto = new ParkingComplexDTO
        {
            Id = result.Id,
            Name = result.Name,
            Address = result.Address,
            Description = result.Description,
            Admins = [],
            ParkingSpaces = [],
        };

        return ServiceResponse.ForSuccess(parkingComplexDto);
    }

    public async Task<ServiceResponse<ParkingComplexDTO>> UpdateParkingComplex(ParkingComplexUpdateDTO parkingComplex, CancellationToken cancellationToken = default)
    {
        var dto = await repository.GetAsync(new ParkingComplexProjectionSpec(parkingComplex.Name), cancellationToken);
        if (dto == null)
        {
            return ServiceResponse.FromError<ParkingComplexDTO>(CommonErrors.ParkingComplexNotFound);
        }

        dto.Name = parkingComplex.Name ?? dto.Name;
        dto.Address = parkingComplex.Address ?? dto.Address;
        dto.Description = parkingComplex.Description ?? dto.Description;

        var entity = await repository.GetAsync(new ParkingComplexSpec(dto.Id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError<ParkingComplexDTO>(CommonErrors.TechnicalSupport); // This should not happen
        }

        entity.Name = parkingComplex.Name ?? entity.Name;
        entity.Address = parkingComplex.Address ?? entity.Address;
        entity.Description = parkingComplex.Description ?? entity.Description;

        await repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess(dto);
    }

    public async Task<ServiceResponse> DeleteParkingComplex(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(new ParkingComplexSpec(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.ParkingComplexNotFound);
        }

        await repository.DeleteAsync<ParkingComplex>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse<ParkingComplexDTO>> AddAdmin(Guid id, Guid adminId, CancellationToken cancellationToken = default)
    {
        ParkingComplex? entity = await repository.GetAsync(new ParkingComplexSpec(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError<ParkingComplexDTO>(CommonErrors.ParkingComplexNotFound);
        }

        User? admin = await repository.GetAsync(new UserSpec(adminId), cancellationToken);
        if (admin == null)
        {
            return ServiceResponse.FromError<ParkingComplexDTO>(CommonErrors.UserNotFound);
        }

        if (admin.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError<ParkingComplexDTO>(CommonErrors.UserNotAdmin);
        }

        if (entity.Admins == null)
        {
            entity.Admins = [];
        }

        if (entity.Admins.Any(a => a.Id == adminId))
        {
            return ServiceResponse.FromError<ParkingComplexDTO>(CommonErrors.UserAlreadyExists);
        }

        entity.Admins.Add(admin);
        await repository.UpdateAsync(entity, cancellationToken);

        var entityDto = await repository.GetAsync(new ParkingComplexProjectionSpec(id), cancellationToken);
        if (entityDto == null)
        {
            return ServiceResponse.FromError<ParkingComplexDTO>(CommonErrors.TechnicalSupport); // This should not happen
        }

        return ServiceResponse.ForSuccess(entityDto);
    }
}


