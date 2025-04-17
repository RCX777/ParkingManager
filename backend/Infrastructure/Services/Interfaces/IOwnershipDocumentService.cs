using ParkingManager.Core.DataTransferObjects;
using ParkingManager.Core.Requests;
using ParkingManager.Core.Responses;

namespace ParkingManager.Infrastructure.Services.Interfaces;

public interface IOwnershipDocumentService
{
    Task<ServiceResponse<PagedResponse<OwnershipDocumentDTO>>> GetOwnershipDocuments(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    Task<ServiceResponse<OwnershipDocumentDTO>> GetOwnershipDocument(Guid id, CancellationToken cancellationToken = default);
}
