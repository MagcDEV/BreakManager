using Break.Api.Mapping;
using Break.Application.Services;
using Break.Contracts.Requests;
using Break.Contracts.Responses; // Add this for ItemResponse
using System.Text.Json; // Add this for JsonSerializer
using Microsoft.AspNetCore.Http.Json; // Add this for JsonOptions
using Microsoft.Extensions.Options;
using Break.Application.Models; // Add this for IOptions

namespace Break.Api.Endpoints;

public static class ItemEndpoints
{
    public static void MapItemEndpoints(this IEndpointRouteBuilder app)
    {

        app.MapGet(ApiEnpoints.Item.GetItem, async (int id, IItemService itemService) =>
        {
            var item = await itemService.GetItemAsync(id);
            // Use pattern matching for a slightly cleaner null check
            return item is not null
                ? Results.Ok(item.MapToItemResponse())
                : Results.NotFound();
        })
        .WithName("GetItem")
        .Produces<ItemResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // --- UPDATE GetAllItems Endpoint ---
        app.MapGet(ApiEnpoints.Item.GetAllItems,
            // Use [AsParameters] for cleaner binding of query parameters (PaginationRequest)
            // Inject HttpResponse to add headers
            // Inject JsonOptions for consistent serialization
            // Inject CancellationToken for potential cancellation
            async ([AsParameters] PaginationRequest pagination,
                   IItemService itemService,
                   HttpResponse response,
                   IOptions<JsonOptions> jsonOptions,
                   CancellationToken cancellationToken) =>
            {
                // Call the service with pagination parameters and cancellation token
                var pagedResult = await itemService.GetItemsAsync(
                    pagination.PageNumber,
                    pagination.PageSize,
                    cancellationToken);

                // Add pagination metadata to the response header
                // Use the injected jsonOptions for consistency with API serialization settings
                response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.Metadata, jsonOptions.Value.SerializerOptions));

                // Map only the items for the current page to the response DTO
                var itemResponses = pagedResult.Items.Select(item => item.MapToItemResponse());

                // Return the list of items for the current page
                return Results.Ok(itemResponses);
            })
        .WithName("GetAllItems")
        // Update Produces to reflect the response body type and potential status codes
        .Produces<IEnumerable<ItemResponse>>(StatusCodes.Status200OK);
        // Note: A 404 is less likely here unless the *concept* of items doesn't exist.
        // An empty list is a valid 200 OK response for no items found on a page or in total.

        app.MapPut(ApiEnpoints.Item.UpdateItem,
            // Add route parameter {id:int}
            async (int id, UpdateItemRequest request, IItemService itemService) =>
            {
                // Call the service, passing the id and request DTO directly
                var updatedItem = await itemService.UpdateItemAsync(id, request);

                // Check if the update was successful (item found and updated)
                return updatedItem is not null
                    ? Results.Ok(updatedItem.MapToItemResponse()) // Map the result from the service
                    : Results.NotFound(); // Return 404 if the item wasn't found by the service
            })
        .WithName("UpdateItem")
        .Produces<ItemResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound) // If item to update doesn't exist
        .ProducesValidationProblem(); // If validation fails (ASP.NET Core handles this automatically for DTOs)

        // --- UPDATE DeleteItem Endpoint ---
        app.MapDelete(ApiEnpoints.Item.DeleteItem, async (int id, IItemService itemService) =>
        {
            var deleted = await itemService.DeleteItemAsync(id);

            // Return NoContent (204) on successful deletion
            // Return NotFound (404) if the item didn't exist
            return deleted
                ? Results.NoContent()
                : Results.NotFound();
        })
        .WithName("DeleteItem")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}