namespace DirectoryService.Shared;

public enum ErrorType
{
    /// <summary>
    /// Ошибка с валидацией.
    /// </summary>
    Validation,
    
    /// <summary>
    /// Ошибка ничего не найдено.
    /// </summary>
    NotFound,
    
    /// <summary>
    /// Ошибка сервера.
    /// </summary>
    Failure,
    
    /// <summary>
    /// Ошибка конфликт.
    /// </summary>
    Conflict
}