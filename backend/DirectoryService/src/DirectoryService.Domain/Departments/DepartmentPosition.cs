using ErrorOr;

namespace DirectoryService.Domain.Departments;

public class DepartmentPosition
{
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
            return Error.Validation("DepartmentPosition.InvalidId", "Id cannot be empty");
        }
        
        if (departmentId == Guid.Empty)
        {
            return Error.Validation("DepartmentPosition.InvalidDepartmentId", "DepartmentId cannot be empty");
        }
        
        if (positionId == Guid.Empty)
        {
            return Error.Validation("DepartmentPosition.InvalidPositionId", "PositionId cannot be empty");
        }
        
        return new DepartmentPosition(id, departmentId,  positionId);
    }
}