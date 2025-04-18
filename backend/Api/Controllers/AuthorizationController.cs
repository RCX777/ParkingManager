using Microsoft.AspNetCore.Mvc;
using ParkingManager.Core.DataTransferObjects;
using ParkingManager.Core.Handlers;
using ParkingManager.Core.Responses;
using ParkingManager.Infrastructure.Authorization;
using ParkingManager.Infrastructure.Services.Interfaces;

namespace ParkingManager.Api.Controllers;

/// <summary>
/// This is a controller to respond to authentication requests.
/// Inject the required services through the constructor.
/// </summary>
[ApiController] // This attribute specifies for the framework to add functionality to the controller such as binding multipart/form-data.
[Route("api/[controller]/[action]")] // The Route attribute prefixes the routes/url paths with template provides as a string, the keywords between [] are used to automatically take the controller and method name.
public class AuthorizationController(IUserService userService) : BaseResponseController // The controller must inherit ControllerBase or its derivations, in this case BaseResponseController.
{
    /// <summary>
    /// This method will respond to login requests.
    /// </summary>
    [HttpPost] // This attribute will make the controller respond to a HTTP POST request on the route /api/Authorization/Login having a JSON body deserialized as a LoginDTO.
    public async Task<ActionResult<RequestResponse<LoginResponseDTO>>> Login([FromBody] LoginDTO login) // The FromBody attribute indicates that the parameter is deserialized from the JSON body.
    {
        return FromServiceResponse(await userService.Login(login with { Password = PasswordUtils.HashPassword(login.Password)})); // The "with" keyword works only with records and it creates another object instance with the updated properties.
    }

    [HttpPost]
    public async Task<ActionResult<RequestResponse<LoginResponseDTO>>> Register([FromBody] RegisterDTO register)
    {
        return FromServiceResponse(await userService.Register(register with { Password = PasswordUtils.HashPassword(register.Password) }));
    }
}
