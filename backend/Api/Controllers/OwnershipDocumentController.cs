using Microsoft.AspNetCore.Mvc;
using ParkingManager.Core.DataTransferObjects;
using ParkingManager.Core.Requests;
using ParkingManager.Core.Responses;
using ParkingManager.Infrastructure.Authorization;
using ParkingManager.Infrastructure.Services.Interfaces;

namespace ParkingManager.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class OwnershipDocumentController(IOwnershipDocumentService parkingSpaceService, IUserService userService) : AuthorizedController(userService)
{
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<OwnershipDocumentDTO>>>> GetOwnershipDocuments([FromQuery] PaginationSearchQueryParams pagination)
    {
        return FromServiceResponse(await parkingSpaceService.GetOwnershipDocuments(pagination));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<OwnershipDocumentDTO>>> GetOwnershipDocument(Guid id)
    {
        return FromServiceResponse(await parkingSpaceService.GetOwnershipDocument(id));
    }
}

