namespace Break.Application.Models; // Or Break.Application.Common if you moved it

/// <summary>
/// Represents a list of items for a specific page, along with pagination metadata.
/// </summary>
/// <typeparam name="T">The type of items in the list.</typeparam>
public class PagedList<T>
{
    /// <summary>
    /// Gets the items for the current page.
    /// </summary>
    public IReadOnlyList<T> Items { get; }

    /// <summary>
    /// Gets the pagination metadata.
    /// </summary>
    public PaginationMetadata Metadata { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
    /// </summary>
    /// <param name="items">The items for the current page.</param>
    /// <param name="metadata">The pagination metadata.</param>
    public PagedList(IReadOnlyList<T> items, PaginationMetadata metadata)
    {
        Items = items;
        Metadata = metadata;
    }
}