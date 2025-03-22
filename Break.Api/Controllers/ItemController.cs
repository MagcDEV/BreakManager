using Break.Api.Mapping;
using Break.Application.Services;
using Break.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Break.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemController(IItemService itemService) : ControllerBase
{

    [HttpPost("AddItem")]
    public async Task<IActionResult> CreateItemAsync([FromBody] CreateItemRequest request)
    {
        var item = request.MapToItem();
        var result = await itemService.AddItemAsync(item);
        if (result == null)
        {
            return BadRequest();
        }
        return Ok(result.MapToItemResponse());
    }

}
