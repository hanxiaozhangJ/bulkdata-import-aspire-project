namespace BulkDataImport.Api;

/// <summary>
/// Simple request DTO for validation demonstration
/// </summary>
public record ValidationRequest
{
    /// <summary>
    /// User's name - must be provided and between 2-50 characters
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// User's email address - must be a valid email format
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// User's age - must be between 18 and 120
    /// </summary>
    public int Age { get; init; }
}

