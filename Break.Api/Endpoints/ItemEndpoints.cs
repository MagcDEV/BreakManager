using Break.Api.Mapping;
using Break.Application.Services;
using Break.Contracts.Requests;
using Break.Contracts.Responses;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using Break.Application.Models; // Needed for Item model
using FluentValidation;
using Microsoft.AspNetCore.Routing; // Needed for LinkGenerator

namespace Break.Api.Endpoints;

public static class ItemEndpoints
{
    public static void MapItemEndpoints(this IEndpointRouteBuilder app)
    {
        var itemGroup = app.MapGroup("")
                           .WithTags("Items");

        // POST /api/item
        itemGroup.MapPost(ApiEnpoints.Item.CreateItem,
            async (CreateItemRequest request, IItemService itemService, LinkGenerator linker) =>
            {
                // 1. Map CreateItemRequest DTO to Item domain model
                var newItem = request.MapToItem();

                // 2. Call the service with the mapped Item object
                var createdItem = await itemService.AddItemAsync(newItem);

                // 3. Map the result back to a response DTO
                var itemResponse = createdItem.MapToItemResponse();

                // 4. Generate the Location header URI
                var locationUri = linker.GetPathByName("GetItem", new { id = createdItem.ItemId });

                // 5. Return 201 Created
                return Results.Created(locationUri, itemResponse);
            })
        .WithName("CreateItem")
        .Produces<ItemResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        // GET /api/item/{id}
        itemGroup.MapGet(ApiEnpoints.Item.GetItem, async (int id, IItemService itemService) =>
        {
            var item = await itemService.GetItemAsync(id);
            return item is not null
                ? Results.Ok(item.MapToItemResponse())
                : Results.NotFound();
        })
        .WithName("GetItem")
        .Produces<ItemResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // GET /api/item/barcode/{barcode}
        itemGroup.MapGet(ApiEnpoints.Item.GetItemByBarcode, async (string barcode, IItemService itemService) =>
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                return Results.BadRequest("Barcode cannot be empty.");
            }
            var item = await itemService.GetItemByBarcodeAsync(barcode);
            return item is not null
                ? Results.Ok(item.MapToItemResponse())
                : Results.NotFound($"Item with barcode '{barcode}' not found.");
        })
        .WithName("GetItemByBarcode")
        .Produces<ItemResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);

        // GET /api/item
        itemGroup.MapGet(ApiEnpoints.Item.GetAllItems,
            async ([AsParameters] PaginationRequest pagination,
                   IItemService itemService,
                   HttpResponse response,
                   IOptions<JsonOptions> jsonOptions,
                   CancellationToken cancellationToken) =>
            {
                var pagedResult = await itemService.GetItemsAsync(
                    pagination.PageNumber,
                    pagination.PageSize,
                    cancellationToken);

                response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.Metadata, jsonOptions.Value.SerializerOptions));
                var itemResponses = pagedResult.Items.Select(item => item.MapToItemResponse());
                return Results.Ok(itemResponses);
            })
        .WithName("GetAllItems")
        .Produces<IEnumerable<ItemResponse>>(StatusCodes.Status200OK);

        // PUT /api/item/{id}
        itemGroup.MapPut(ApiEnpoints.Item.UpdateItem,
            async (int id, UpdateItemRequest request, IItemService itemService) =>
            {
                var updatedItem = await itemService.UpdateItemAsync(id, request);
                return updatedItem is not null
                    ? Results.Ok(updatedItem.MapToItemResponse())
                    : Results.NotFound();
            })
        .WithName("UpdateItem")
        .Produces<ItemResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem();

        // DELETE /api/item/{id}
        itemGroup.MapDelete(ApiEnpoints.Item.DeleteItem, async (int id, IItemService itemService) =>
        {
            var deleted = await itemService.DeleteItemAsync(id);
            return deleted
                ? Results.NoContent()
                : Results.NotFound();
        })
        .WithName("DeleteItem")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}