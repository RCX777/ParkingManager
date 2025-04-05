using System.Net;

namespace ParkingManager.Core.Errors;

/// <summary>
/// This is an alternative solution to the ServiceResponse class if you rather want to use exceptions.
/// You may add more exceptions for other HTTP codes as you like.
/// </summary>
public class ServerException(
    HttpStatusCode status,
    string message,
    ErrorCodes code = ErrorCodes.Unknown) : Exception(message)
{
    public HttpStatusCode Status { get; } = status;
    public ErrorCodes Code { get; } = code;
}

public class InternalServerErrorException(
    string message = "Something went wrong!",
    ErrorCodes code = ErrorCodes.Unknown) : ServerException(HttpStatusCode.InternalServerError, message, code)
{
}

public class ForbiddenException(
    string message,
    ErrorCodes code = ErrorCodes.Unknown) : ServerException(HttpStatusCode.Forbidden, message, code)
{
}

public class BadRequestException(
    string message,
    ErrorCodes code = ErrorCodes.Unknown) : ServerException(HttpStatusCode.BadRequest, message, code)
{
}

public class UnauthorizedException(
    string message,
    ErrorCodes code = ErrorCodes.Unknown) : ServerException(HttpStatusCode.Unauthorized, message, code)
{
}

public class NotFoundException(
    string message,
    ErrorCodes code = ErrorCodes.Unknown) : ServerException(HttpStatusCode.NotFound, message, code)
{
}

public class ServiceUnavailableException(
    string message,
    ErrorCodes code = ErrorCodes.Unknown) : ServerException(HttpStatusCode.ServiceUnavailable, message, code)
{
}

public class ConflictException(
    string message,
    ErrorCodes code = ErrorCodes.Unknown) : ServerException(HttpStatusCode.Conflict, message, code)
{
}
