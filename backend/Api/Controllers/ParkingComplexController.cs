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
public class ParkingComplexController(IParkingComplexService parkingComplexService, IUserService userService) : AuthorizedController(userService)
{
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ParkingComplexDTO>>>> GetParkingComplexes([FromQuery] PaginationSearchQueryParams pagination)
    {
        return FromServiceResponse(await parkingComplexService.GetParkingComplexes(pagination));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<ParkingComplexDTO>>> GetParkingComplex(Guid id)
    {
        return FromServiceResponse(await parkingComplexService.GetParkingComplex(id));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse<ParkingComplexDTO>>> CreateParkingComplex([FromBody] ParkingComplexAddDTO parkingComplex)
    {
        var user = await GetCurrentUser();
        if (user.Result?.Role != UserRoleEnum.Admin)
        {
            return ErrorMessageResult<ParkingComplexDTO>(CommonErrors.UserNotAdmin);
        }
        return FromServiceResponse(await parkingComplexService.AddParkingComplex(parkingComplex));
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> DeleteParkingComplex(Guid id)
    {
        var user = await GetCurrentUser();
        if (user.Result?.Role != UserRoleEnum.Admin)
        {
            return ErrorMessageResult(CommonErrors.UserNotAdmin);
        }
        return FromServiceResponse(await parkingComplexService.DeleteParkingComplex(id));
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse<ParkingComplexDTO>>> UpdateParkingComplex([FromBody] ParkingComplexUpdateDTO parkingComplex)
    {
        var user = await GetCurrentUser();
        if (user.Result?.Role != UserRoleEnum.Admin)
        {
            return ErrorMessageResult<ParkingComplexDTO>(CommonErrors.UserNotAdmin);
        }
        return FromServiceResponse(await parkingComplexService.UpdateParkingComplex(parkingComplex));
    }

    [Authorize]
    public record AddAdminRequest(Guid Id , Guid AdminId);
    [HttpPost]
    public async Task<ActionResult<RequestResponse<ParkingComplexDTO>>> AddAdmin([FromBody] AddAdminRequest request)
    {
        var user = await GetCurrentUser();
        if (user.Result?.Role != UserRoleEnum.Admin)
        {
            return ErrorMessageResult<ParkingComplexDTO>(CommonErrors.UserNotAdmin);
        }
        return FromServiceResponse(await parkingComplexService.AddAdmin(request.Id, request.AdminId));
    }
}

