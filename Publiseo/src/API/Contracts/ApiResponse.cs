namespace API.Contracts;

public class ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public int StatusCode { get; init; }
    public string? Message { get; init; }
    public IReadOnlyDictionary<string, string[]>? Errors { get; init; }

    public static ApiResponse<T> Ok(T data, int statusCode = 200) => new()
    {
        Success = true,
        Data = data,
        StatusCode = statusCode,
        Message = null,
        Errors = null
    };

    public static ApiResponse<T> Fail(int statusCode, string message, IReadOnlyDictionary<string, string[]>? errors = null) => new()
    {
        Success = false,
        Data = default,
        StatusCode = statusCode,
        Message = message,
        Errors = errors
    };
}
