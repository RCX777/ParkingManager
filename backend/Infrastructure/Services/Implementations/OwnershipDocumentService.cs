using ParkingManager.Core.DataTransferObjects;
using ParkingManager.Core.Errors;
using ParkingManager.Core.Requests;
using ParkingManager.Core.Responses;
using ParkingManager.Core.Specifications;
using ParkingManager.Infrastructure.Database;
using ParkingManager.Infrastructure.Repositories.Interfaces;
using ParkingManager.Infrastructure.Services.Interfaces;

namespace ParkingManager.Infrastructure.Services.Implementations;

public class OwnershipDocumentService(IRepository<WebAppDatabaseContext> repository) : IOwnershipDocumentService
{
    public async Task<ServiceResponse<PagedResponse<OwnershipDocumentDTO>>> GetOwnershipDocuments(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new OwnershipDocumentProjectionSpec(true), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse<OwnershipDocumentDTO>> GetOwnershipDocument(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new OwnershipDocumentProjectionSpec(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<OwnershipDocumentDTO>(CommonErrors.OwnershipDocumentNotFound);
    }
}

