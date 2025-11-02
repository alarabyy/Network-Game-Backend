namespace Application.Common;

public record PaginatedRequest(int Page = 1, int ItemsPerPage = 20);