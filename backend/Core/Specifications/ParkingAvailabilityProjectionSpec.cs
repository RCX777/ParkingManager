using Ardalis.Specification;
using ParkingManager.Core.DataTransferObjects;
using ParkingManager.Core.Entities;

namespace ParkingManager.Core.Specifications;

public class ParkingAvailabilityProjectionSpec : Specification<ParkingAvailability, ParkingAvailabilityDTO>
{
    public ParkingAvailabilityProjectionSpec(bool orderByCreatedAt = false) =>
        Query
            .Select(e => new ParkingAvailabilityDTO
            {
                Id = e.Id,
                EndDate = e.EndDate,
                StartDate = e.StartDate,
                ParkingSpaceId = e.ParkingSpaceId,
            })
            .OrderByDescending(x => x.CreatedAt, orderByCreatedAt);

    public ParkingAvailabilityProjectionSpec(Guid id) : this()
    {
        Query
            .Where(x => x.Id == id);
    }
}

