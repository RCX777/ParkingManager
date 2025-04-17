using System.Net;

namespace ParkingManager.Core.Errors;

/// <summary>
/// Common error messages that may be reused in various places in the code.
/// </summary>
public static class CommonErrors
{
    public static ErrorMessage UserNotFound => new(HttpStatusCode.NotFound, "User doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage FileNotFound => new(HttpStatusCode.NotFound, "File not found on disk!", ErrorCodes.PhysicalFileNotFound);
    public static ErrorMessage TechnicalSupport => new(HttpStatusCode.InternalServerError, "An unknown error occurred, contact the technical support!", ErrorCodes.TechnicalError);
    public static ErrorMessage ParkingComplexNotFound => new(HttpStatusCode.NotFound, "Parking complex doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage UserNotAdmin => new(HttpStatusCode.Forbidden, "User is not an admin!", ErrorCodes.UserNotAdmin);
    public static ErrorMessage UserAlreadyExists => new(HttpStatusCode.Conflict, "User already exists!", ErrorCodes.UserAlreadyExists);
    public static ErrorMessage ParkingSpaceNotFound => new(HttpStatusCode.NotFound, "Parking space doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage CannotAddParkingSpace => new(HttpStatusCode.InternalServerError, "Cannot add parking space!", ErrorCodes.TechnicalError);
    public static ErrorMessage CannotUpdateParkingSpace => new(HttpStatusCode.InternalServerError, "Cannot update parking space!", ErrorCodes.TechnicalError);
    public static ErrorMessage UserNotOwner => new(HttpStatusCode.Forbidden, "User is not the owner of the parking space!", ErrorCodes.UserNotOwner);
    public static ErrorMessage OwnershipDocumentNotFound => new(HttpStatusCode.Forbidden, "Ownership Document Not Found!", ErrorCodes.UserNotOwner);

    public static ErrorMessage? ParkingAvailabilityNotFound { get; internal set; }
}
