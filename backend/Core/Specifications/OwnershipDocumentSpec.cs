using Ardalis.Specification;
using ParkingManager.Core.Entities;

namespace ParkingManager.Core.Specifications;

public class OwnershipDocumentSpec : Specification<OwnershipDocument>
{
    public OwnershipDocumentSpec(Guid id) => Query.Where(e => e.Id == id);
}

