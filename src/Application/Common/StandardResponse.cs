namespace Application.Common;

public record StandardResponse(bool Success, string? Message)
{
    public static StandardResponse Succeeded(string message)
        => new(true, message);

    public static StandardResponse Failed(string message)
        => new(false, message);
}

public record StandardResponse<T>(bool Success, T? Content, string? Message)
{
    public static StandardResponse<T> Succeeded(T? content, string? message = null)
        => new(true, content, message);

    public static StandardResponse<T> Failed(string? message = null)
        => new(false, default, message);
}