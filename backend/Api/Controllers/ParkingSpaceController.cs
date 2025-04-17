using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingManager.Core.DataTransferObjects;
using ParkingManager.Core.Enums;
using ParkingManager.Core.Errors;
using ParkingManager.Core.Requests;
using ParkingManager.Core.Responses;
using ParkingManager.Infrastructure.Authorization;
using ParkingManager.Infrastructure.Services.Interfaces;

namespace ParkingManager.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ParkingSpaceController(IParkingSpaceService parkingSpaceService, IUserService userService) : AuthorizedController(userService)
{
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ParkingSpaceDTO>>>> GetParkingSpaces([FromQuery] PaginationSearchQueryParams pagination)
    {
        return FromServiceResponse(await parkingSpaceService.GetParkingSpaces(pagination));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<ParkingSpaceDTO>>> GetParkingSpace(Guid id)
    {
        return FromServiceResponse(await parkingSpaceService.GetParkingSpace(id));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse<ParkingSpaceDTO>>> CreateParkingSpace([FromBody] ParkingSpaceAddDTO parkingSpace)
    {
        var user = await GetCurrentUser();
        if (user.Result?.Role != UserRoleEnum.Admin)
        {
            return ErrorMessageResult<ParkingSpaceDTO>(CommonErrors.UserNotAdmin);
        }
        return FromServiceResponse(await parkingSpaceService.AddParkingSpace(parkingSpace));
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> DeleteParkingSpace(Guid id)
    {
        var user = await GetCurrentUser();
        if (user.Result?.Role != UserRoleEnum.Admin)
        {
            return ErrorMessageResult(CommonErrors.UserNotAdmin);
        }
        return FromServiceResponse(await parkingSpaceService.DeleteParkingSpace(id));
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse<ParkingSpaceDTO>>> UpdateParkingSpace([FromBody] ParkingSpaceUpdateDTO parkingSpace)
    {
        var user = await GetCurrentUser();
        if (user.Result?.Id != parkingSpace.UserId)
        {
            if (user.Result?.Role != UserRoleEnum.Admin)
            {
                return ErrorMessageResult<ParkingSpaceDTO>(CommonErrors.UserNotAdmin);
            }
            return ErrorMessageResult<ParkingSpaceDTO>(CommonErrors.UserNotOwner);
        }
        return FromServiceResponse(await parkingSpaceService.UpdateParkingSpace(parkingSpace));
    }
}

