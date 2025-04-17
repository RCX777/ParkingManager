using Ardalis.Specification;
using ParkingManager.Core.Entities;

namespace ParkingManager.Core.Specifications;

public class ParkingComplexSpec : Specification<ParkingComplex>
{
    public ParkingComplexSpec(Guid id) => Query.Where(e => e.Id == id);

    public ParkingComplexSpec(string Name) => Query.Where(e => e.Name == Name);

}
