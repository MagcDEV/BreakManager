using Break.Api.Mapping;
using Break.Application.Services;
using Break.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Break.Api.Controllers;

[ApiController]
public class ItemController(IItemService itemService) : ControllerBase
{
    [HttpPost(ApiEnpoints.Item.CreateItem)]
    public async Task<IActionResult> CreateItem([FromBody] CreateItemRequest request)
    {
        var item = request.MapToItem();
        var result = await itemService.AddItemAsync(item);
        if (result == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(GetItem), new { id = result.ItemId }, result.MapToItemResponse());
    }

    [HttpGet(ApiEnpoints.Item.GetItem)]
    public async Task<IActionResult> GetItem(int id)
    {
        var result = await itemService.GetItemAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result.MapToItemResponse());
    }

    [HttpGet(ApiEnpoints.Item.GetAllItems)]
    public async Task<IActionResult> GetAllItems()
    {
        var result = await itemService.GetItemsAsync();
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result.MapToItemResponse());
    }

    [HttpPut(ApiEnpoints.Item.UpdateItem)]
    public async Task<IActionResult> UpdateItem([FromBody] UpdateItemRequest request)
    {
        var item = request.MapToItem();
        var result = await itemService.UpdateItemAsync(item);
        if (result == null)
        {
            return BadRequest();
        }
        return Ok(result.MapToItemResponse());
    }

    [HttpDelete(ApiEnpoints.Item.DeleteItem)]
    public async Task<IActionResult> DeleteItemAsync(int id)
    {
        var result = await itemService.DeleteItemAsync(id);

        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }
}
