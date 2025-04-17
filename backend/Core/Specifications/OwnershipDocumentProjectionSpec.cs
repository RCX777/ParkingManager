using Ardalis.Specification;
using ParkingManager.Core.DataTransferObjects;
using ParkingManager.Core.Entities;

namespace ParkingManager.Core.Specifications;

public class OwnershipDocumentProjectionSpec : Specification<OwnershipDocument, OwnershipDocumentDTO>
{
    public OwnershipDocumentProjectionSpec(bool orderByCreatedAt = false) =>
        Query
            .Select(e => new OwnershipDocumentDTO
            {
                Id = e.Id,
                UserFileId = e.UserFileId,
                ParkingSpaceDTO = e.ParkingSpace != null
                    ? new ParkingSpaceDTO
                    {
                        Id = e.ParkingSpace.Id,
                        UserId = e.ParkingSpace.UserId,
                        Comments = e.ParkingSpace.Comments != null
                            ? e.ParkingSpace.Comments.Select(c => new CommentDTO
                            {
                                Id = c.Id,
                                CreatedAt = c.CreatedAt,
                                Text = c.Text,
                            }).ToList()
                            : new List<CommentDTO>(),
                        Availabilities = e.ParkingSpace.Availabilities != null
                            ? e.ParkingSpace.Availabilities.Select(a => new ParkingAvailabilityDTO
                            {
                                Id = a.Id,
                                StartDate = a.StartDate,
                                EndDate = a.EndDate,
                            }).ToList()
                            : new List<ParkingAvailabilityDTO>(),
                        ParkingComplexId = e.ParkingSpace.ParkingComplexId,
                        OwnershipDocumentId = e.ParkingSpace.OwnershipDocumentId,
                    }
                    : null,
            })
            .OrderByDescending(x => x.CreatedAt, orderByCreatedAt);

    public OwnershipDocumentProjectionSpec(Guid id) : this()
    {
        Query
            .Where(x => x.Id == id);
    }
}

