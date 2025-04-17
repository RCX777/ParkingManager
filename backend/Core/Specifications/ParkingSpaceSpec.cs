using Ardalis.Specification;
using ParkingManager.Core.Entities;

namespace ParkingManager.Core.Specifications;

public class ParkingSpaceSpec : Specification<ParkingSpace>
{
    public ParkingSpaceSpec(Guid id) => Query.Where(e => e.Id == id);
}

