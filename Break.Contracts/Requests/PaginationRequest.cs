namespace Break.Contracts.Requests;

public record PaginationRequest(int PageNumber = 1, int PageSize = 10)
{
    // Basic validation
    public int PageNumber { get; init; } = PageNumber <= 0 ? 1 : PageNumber;
    public int PageSize { get; init; } = PageSize <= 0 ? 10 : PageSize > 100 ? 100 : PageSize; // Max page size 100
}