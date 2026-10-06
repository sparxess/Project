using DirectoryService.Shared;

namespace DirectoryService.Application.Locations.Fails;

public static partial class Errors
{
    public static class LocationErrors
    {
        public static DomainError NotFound(Guid id) =>
            DomainError.NotFound("location.not_found", $"Локация с id: {id} не найдена.", id);
        
        public static DomainError NameExists(string name) =>
            DomainError.Conflict("location.name.exists", $"Локация с именем '{name}' уже существует.");
    }
}