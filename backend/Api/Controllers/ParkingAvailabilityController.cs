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
public class ParkingAvailabilityController(IParkingAvailabilityService parkingAvailabilityService, IUserService userService) : AuthorizedController(userService)
{
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ParkingAvailabilityDTO>>>> GetParkingAvailabilitys([FromQuery] PaginationSearchQueryParams pagination)
    {
        return FromServiceResponse(await parkingAvailabilityService.GetParkingAvailabilitys(pagination));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<ParkingAvailabilityDTO>>> GetParkingAvailability(Guid id)
    {
        return FromServiceResponse(await parkingAvailabilityService.GetParkingAvailability(id));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse<ParkingAvailabilityDTO>>> CreateParkingAvailability([FromBody] ParkingAvailabilityAddDTO parkingAvailability)
    {
        var user = await GetCurrentUser();
        if ((user.Result?.ParkingSpaces?.Any(x => x.Id == parkingAvailability.ParkingSpaceId) ?? false) != true)
        {
            if (user.Result?.Role == UserRoleEnum.Admin)
            {
                return FromServiceResponse(await parkingAvailabilityService.AddParkingAvailability(parkingAvailability));
            }
            return ErrorMessageResult<ParkingAvailabilityDTO>(CommonErrors.UserNotAdmin);
        }
        return FromServiceResponse(await parkingAvailabilityService.AddParkingAvailability(parkingAvailability));
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> DeleteParkingAvailability(Guid id)
    {
        var user = await GetCurrentUser();
        if (user.Result?.ParkingSpaces?.Any(x => x?.Availabilities?.Any(a => a?.Id == id) ?? false) != true)
        {
            if (user.Result?.Role == UserRoleEnum.Admin)
            {
                return FromServiceResponse(await parkingAvailabilityService.DeleteParkingAvailability(id));
            }
            return ErrorMessageResult(CommonErrors.UserNotAdmin);
        }
        return FromServiceResponse(await parkingAvailabilityService.DeleteParkingAvailability(id));
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse<ParkingAvailabilityDTO>>> UpdateParkingAvailability([FromBody] ParkingAvailabilityUpdateDTO parkingAvailability)
    {
        var user = await GetCurrentUser();
        if (user.Result?.ParkingSpaces?.Any(x => x?.Availabilities?.Any(a => a?.Id == parkingAvailability.Id) ?? false) != true)
        {
            if (user.Result?.Role == UserRoleEnum.Admin)
            {
                return FromServiceResponse(await parkingAvailabilityService.UpdateParkingAvailability(parkingAvailability));
            }
            return ErrorMessageResult<ParkingAvailabilityDTO>(CommonErrors.UserNotAdmin);
        }
        return FromServiceResponse(await parkingAvailabilityService.UpdateParkingAvailability(parkingAvailability));
    }
}

