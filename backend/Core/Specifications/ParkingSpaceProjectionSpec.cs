using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using ParkingManager.Core.DataTransferObjects;
using ParkingManager.Core.Entities;

namespace ParkingManager.Core.Specifications;

public class ParkingSpaceProjectionSpec : Specification<ParkingSpace, ParkingSpaceDTO>
{
    public ParkingSpaceProjectionSpec(bool orderByCreatedAt = false) =>
        Query
            .Select(e => new ParkingSpaceDTO
            {
                Id = e.Id,
                ParkingComplexId = e.ParkingComplexId,
                UserId = e.UserId,
                OwnershipDocumentId = e.OwnershipDocumentId,
                Comments = e.Comments.Select(c => new CommentDTO
                {
                    Id = c.Id,
                    Text = c.Text,
                    CreatedAt = c.CreatedAt,
                }).ToList(),
                Availabilities = e.Availabilities.Select(a => new ParkingAvailabilityDTO
                {
                    Id = a.Id,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                }).ToList(),
            })
            .OrderByDescending(x => x.CreatedAt, orderByCreatedAt);

    public ParkingSpaceProjectionSpec(Guid id) : this()
    {
        Query
            .Include(x => x.Availabilities)
            .Include(x => x.Comments)
            .Where(x => x.Id == id);
    }
}

