﻿using System.Net;
using System.ComponentModel.DataAnnotations;
using ParkingManager.Core.Constants;
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

/// <summary>
/// Inject the required services through the constructor.
/// </summary>
public class UserService(IRepository<WebAppDatabaseContext> repository, ILoginService loginService, IMailService mailService)
    : IUserService
{
    public async Task<ServiceResponse<UserDTO>> GetUser(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new UserProjectionSpec(id), cancellationToken); // Get a user using a specification on the repository.

        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<UserDTO>(CommonErrors.UserNotFound); // Pack the result or error into a ServiceResponse.
    }

    public async Task<ServiceResponse<PagedResponse<UserDTO>>> GetUsers(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new UserProjectionSpec(pagination.Search), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<LoginResponseDTO>> Login(LoginDTO login, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new UserSpec(login.Email), cancellationToken);

        if (result == null) // Verify if the user is found in the database.
        {
            return ServiceResponse.FromError<LoginResponseDTO>(CommonErrors.UserNotFound); // Pack the proper error as the response.
        }

        if (result.Password != login.Password) // Verify if the password hash of the request is the same as the one in the database.
        {
            return ServiceResponse.FromError<LoginResponseDTO>(new(HttpStatusCode.BadRequest, "Wrong password!", ErrorCodes.WrongPassword));
        }

        var user = new UserDTO
        {
            Id = result.Id,
            Email = result.Email,
            Name = result.Name,
            Role = result.Role
        };

        return ServiceResponse.ForSuccess(new LoginResponseDTO
        {
            User = user,
            Token = loginService.GetToken(user, DateTime.UtcNow, new(7, 0, 0, 0)) // Get a JWT for the user issued now and that expires in 7 days.
        });
    }

    public async Task<ServiceResponse<LoginResponseDTO>> Register(RegisterDTO register, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new UserSpec(register.Email), cancellationToken);

        if (result != null) // Verify if the user is found in the database.
        {
            return ServiceResponse.FromError<LoginResponseDTO>(new(HttpStatusCode.Conflict, "The user already exists!", ErrorCodes.UserAlreadyExists));
        }

        if (!new EmailAddressAttribute().IsValid(register.Email)) // Verify if the email is valid.
        {
            return ServiceResponse.FromError<LoginResponseDTO>(new(HttpStatusCode.BadRequest, "The email is not valid!", ErrorCodes.InvalidEmail));
        }

        var user = new User
        {
            Email = register.Email,
            Name = register.Name,
            Role = UserRoleEnum.User,
            Password = register.Password
        };

        await repository.AddAsync(user, cancellationToken); // A new entity is created and persisted in the database.

        var userDTO = new UserDTO
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Role = user.Role
        };

        var emailResult = await mailService.SendMail(register.Email, "Welcome to ParkingManager!", MailTemplates.UserAddTemplate(register.Name), true, "ParkingManager App", cancellationToken); // You can send a notification on the user email. Change the email if you want.

        if (emailResult == null) // Verify if the email was sent.
        {
            return ServiceResponse.FromError<LoginResponseDTO>(new(HttpStatusCode.InternalServerError, "The email could not be sent!", ErrorCodes.EmailNotSent));
        }

        return ServiceResponse.ForSuccess(new LoginResponseDTO
        {
            User = userDTO,
            Token = loginService.GetToken(userDTO, DateTime.UtcNow, new(7, 0, 0, 0))
        });
    }

    public async Task<ServiceResponse<int>> GetUserCount(CancellationToken cancellationToken = default) =>
        ServiceResponse.ForSuccess(await repository.GetCountAsync<User>(cancellationToken)); // Get the count of all user entities in the database.

    public async Task<ServiceResponse> AddUser(UserAddDTO user, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can add users!", ErrorCodes.CannotAdd));
        }

        var result = await repository.GetAsync(new UserSpec(user.Email), cancellationToken);

        if (result != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The user already exists!", ErrorCodes.UserAlreadyExists));
        }

        if (!new EmailAddressAttribute().IsValid(user.Email)) // Verify if the email is valid.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.BadRequest, "The email is not valid!", ErrorCodes.InvalidEmail));
        }

        await repository.AddAsync(new User
        {
            Email = user.Email,
            Name = user.Name,
            Role = user.Role,
            Password = user.Password
        }, cancellationToken); // A new entity is created and persisted in the database.

        await mailService.SendMail(user.Email, "Welcome!", MailTemplates.UserAddTemplate(user.Name), true, "ParkingManager App", cancellationToken); // You can send a notification on the user email. Change the email if you want.

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateUser(UserUpdateDTO user, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != user.Id) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the own user can update the user!", ErrorCodes.CannotUpdate));
        }

        var entity = await repository.GetAsync(new UserSpec(user.Id), cancellationToken);

        if (entity != null) // Verify if the user is not found, you cannot update a non-existing entity.
        {
            entity.Name = user.Name ?? entity.Name;
            entity.Password = user.Password ?? entity.Password;

            await repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteUser(Guid id, UserDTO? requestingUser = null, CancellationToken cancellationToken = default)
    {
        if (requestingUser == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Must be logged in to delete a user!", ErrorCodes.CannotDelete));
        }
        if (requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != id) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the own user can delete the user!", ErrorCodes.CannotDelete));
        }

        await repository.DeleteAsync<User>(id, cancellationToken); // Delete the entity.

        return ServiceResponse.ForSuccess();
    }
}
