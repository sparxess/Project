using ErrorOr;

namespace DirectoryService.Domain.Departments;

public class DepartmentPosition
{
    // EF Core
    private DepartmentPosition() { }
    
    private DepartmentPosition(Guid id, Guid departmentId, Guid positionId)
    {
        Id = id;
        DepartmentId = departmentId;
        PositionId = positionId;
    }
    
    public Guid Id { get; }
    public Guid DepartmentId { get; }
    public Guid PositionId { get; }

    public static ErrorOr<DepartmentPosition> Create(Guid id, Guid departmentId, Guid positionId)
    {
        if (id == Guid.Empty)
        {
            return Error.Validation(
                "department_position.invalid_id",
                "Id не может быть пустым.");
        }
        
        if (departmentId == Guid.Empty)
        {
            return Error.Validation(
                "department_position.invalid_department_id",
                "DepartmentId не может быть пустым.");
        }
        
        if (positionId == Guid.Empty)
        {
            return Error.Validation(
                "department_position.invalid_position_id",
                "PositionId не может быть пустым.");
        }
        
        return new DepartmentPosition(id, departmentId,  positionId);
    }
}
