using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using ParkingManager.Core.DataTransferObjects;
using ParkingManager.Core.Entities;

namespace ParkingManager.Core.Specifications;

public class ParkingComplexProjectionSpec : Specification<ParkingComplex, ParkingComplexDTO>
{
    public ParkingComplexProjectionSpec(bool orderByCreatedAt = false) =>
        Query
            .Select(e => new ParkingComplexDTO
            {
                Id = e.Id,
                Name = e.Name,
                Address = e.Address,
                Description = e.Description,
                Admins = e.Admins.Select(a => new UserDTO
                {
                    Id = a.Id,
                    Email = a.Email,
                    Name = a.Name,
                    Role = a.Role
                }).ToList(),
            })
            .OrderByDescending(x => x.CreatedAt, orderByCreatedAt);

    public ParkingComplexProjectionSpec(string? searchString) : this(true)
    {
        searchString = !string.IsNullOrWhiteSpace(searchString) ? searchString.Trim() : null;

        if (searchString == null)
        {
            return;
        }

        var searchExpr = $"%{searchString.Replace(" ", "%")}%";

        Query.Where(e => EF.Functions.ILike(e.Name, searchExpr)); // This is an example on how database specific expressions can be used via C# expressions.
    }

    public ParkingComplexProjectionSpec(Guid id) : this()
    {
        Query
            .Include(x => x.Admins)
            .Include(x => x.ParkingSpaces)
            .Where(x => x.Id == id);
    }
}

