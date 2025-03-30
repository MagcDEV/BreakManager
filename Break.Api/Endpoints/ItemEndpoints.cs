using Break.Api.Mapping;
using Break.Application.Services;
using Break.Contracts.Requests;

namespace Break.Api.Endpoints;

public static class ItemEndpoints
{
    public static void MapItemEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEnpoints.Item.CreateItem, async (CreateItemRequest request, IItemService itemService) =>
        {
            var item = request.MapToItem();
            var result = await itemService.AddItemAsync(item);
            if (result == null)
                return Results.BadRequest();

            return Results.Created($"/items/{result.ItemId}", result.MapToItemResponse());
        })
        .WithName("CreateItem");

        app.MapGet(ApiEnpoints.Item.GetItem, async (int id, IItemService itemService) =>
        {
            var result = await itemService.GetItemAsync(id);
            if (result == null)
                return Results.NotFound();

            return Results.Ok(result.MapToItemResponse());
        })
        .WithName("GetItem");

        app.MapGet(ApiEnpoints.Item.GetAllItems, async (IItemService itemService) =>
        {
            var result = await itemService.GetItemsAsync();
            if (result == null)
                return Results.NotFound();

            return Results.Ok(result.MapToItemResponse());
        })
        .WithName("GetAllItems");

        app.MapPut(ApiEnpoints.Item.UpdateItem, async (UpdateItemRequest request, IItemService itemService) =>
        {
            var item = request.MapToItem();
            var result = await itemService.UpdateItemAsync(item);
            if (result == null)
                return Results.BadRequest();

            return Results.Ok(result.MapToItemResponse());
        })
        .WithName("UpdateItem");

        app.MapDelete(ApiEnpoints.Item.DeleteItem, async (int id, IItemService itemService) =>
        {
            var result = await itemService.DeleteItemAsync(id);
            if (!result)
                return Results.BadRequest();

            return Results.Ok();
        })
        .WithName("DeleteItem");
    }
}
