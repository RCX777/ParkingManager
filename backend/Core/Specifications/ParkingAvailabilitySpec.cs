using Ardalis.Specification;
using ParkingManager.Core.Entities;

namespace ParkingManager.Core.Specifications;

public class ParkingAvailabilitySpec : Specification<ParkingAvailability>
{
    public ParkingAvailabilitySpec(Guid id) => Query.Where(e => e.Id == id);
}

