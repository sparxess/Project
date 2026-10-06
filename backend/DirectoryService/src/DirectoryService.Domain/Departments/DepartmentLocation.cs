using ErrorOr;

namespace DirectoryService.Domain.Departments;

public class DepartmentLocation
{
    // EF Core
    private DepartmentLocation() { }
    
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
            return Error.Validation(
                "department_location.invalid_id",
                "Id не может быть пустым.");
        }
        
        if (departmentId == Guid.Empty)
        {
            return Error.Validation(
                "department_location.invalid_department_id",
                "DepartmentId не может быть пустым.");
        }
        
        if (locationId == Guid.Empty)
        {
            return Error.Validation(
                "department_location.invalid_location_id",
                "LocationId не может быть пустым.");
        }
        
        return new DepartmentLocation(id, departmentId, locationId, isPrimary);
    }
}
