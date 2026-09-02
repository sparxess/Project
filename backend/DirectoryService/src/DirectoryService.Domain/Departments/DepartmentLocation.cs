using ErrorOr;

namespace DirectoryService.Domain.Departments;

public class DepartmentLocation
{
    private DepartmentLocation(Guid id, Guid departmentId, Guid locationId, bool isPrimary)
    {
        Id  = id;
        DepartmentId = departmentId;
        LocationId =  locationId;
        IsPrimary = isPrimary;
    }
    
    public Guid Id { get; }
    public Guid DepartmentId { get; }
    public Guid LocationId { get; }
    public bool IsPrimary { get; private set; }
    
    public static ErrorOr<DepartmentLocation> Create(Guid id, Guid departmentId, Guid locationId, bool isPrimary = false)
    {
        if (id == Guid.Empty)
        {
            return Error.Validation("DepartmentLocation.InvalidId", "Id cannot be empty");
        }
        
        if (departmentId == Guid.Empty)
        {
            return Error.Validation("DepartmentLocation.InvalidDepartmentId", "DepartmentId cannot be empty");
        }
        
        if (locationId == Guid.Empty)
        {
            return Error.Validation("DepartmentLocation.InvalidLocationId", "LocationId cannot be empty");
        }
        
        return new DepartmentLocation(id, departmentId, locationId, isPrimary);
    }
}