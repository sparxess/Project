using DirectoryService.Shared;

namespace DirectoryService.Application.Departments.Fails;

public static partial class Errors
{
    public static class DepartmentErrors
    {
        public static DomainError SlugExists(string slug) =>
            DomainError.Conflict(
                "department.slug.exists",
                $"Slug {slug} уже занят.");
        
        public static DomainError NotFound(Guid id) =>
            DomainError.NotFound(
                "department.not_found",
                $"Подразделение {id} не найдено.",
                id);
        
        public static DomainError ParentNotFound(Guid id) =>
            DomainError.NotFound(
                "department.parent.not_found",
                $"Родительское подразделение с id: '{id}' не найдено.",
                id);
        
        public static DomainError LocationAlreadyExists(Guid id) =>
            DomainError.Conflict(
                "department.location.duplicate",
                $"Указанная локация с id: {id} уже привязана к данному подразделению.");

        public static DomainError LocationsNotFound() =>
            DomainError.NotFound(
                "department.locations.not_found",
                $"Одна или несколько указанных локаций не существуют.",
                null);
        
        public static DomainError LocationNotFound(Guid id) =>
            DomainError.NotFound(
                "department.location.not_found",
                $"Указанная локация c id: {id} не существует.",
                id);

        public static DomainError LocationNotLinked(Guid id) =>
            DomainError.NotFound(
                "department.location.not_linked",
                $"Локация id: {id} не привязана к указанному подразделению или не существует.",
                id);
    }
}