using Break.Api.Mapping;
using Break.Application.Services;
using Break.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Break.Api.Controllers;

[ApiController]
public class SaleController(ISaleService saleService) : ControllerBase
{
    [HttpPost(ApiEnpoints.Sale.CreateSale)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var saleItems = request.MapToSaleItems().ToList();

        var sale = await saleService.CreateSaleAsync(saleItems, request.CouponCode);

        return CreatedAtAction(
            nameof(CreateSale),
            new { id = sale.SaleId },
            new { SaleId = sale.SaleId, Total = sale.Total }
        );
    }
}
