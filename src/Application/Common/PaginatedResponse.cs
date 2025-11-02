namespace Application.Common;

public record PaginatedResponse<T>(List<T>? Items, int PageCount);