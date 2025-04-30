using Break.Api.Mapping;
using Break.Application.Services;
using Break.Contracts.Requests;

namespace Break.Api.Endpoints;

public static class SaleEndpoints
{
    public static void MapSaleEndpoints(this IEndpointRouteBuilder app)
    {
        var saleGroup = app.MapGroup("")
            .WithTags("Sales");

        saleGroup.MapPost(
                ApiEnpoints.Sale.CreateSale,
                async (CreateSaleRequest request, ISaleService saleService) =>
                {
                    var saleItems = request.MapToSaleItems().ToList();
                    var sale = await saleService.CreateSaleAsync(saleItems, request.CouponCode);

                    return Results.Created(
                        $"/sales/{sale.SaleId}",
                        new { SaleId = sale.SaleId, Total = sale.Total }
                    );
                }
            )
            .WithName("CreateSale");

        saleGroup.MapGet(
                ApiEnpoints.Sale.GetSale,
                async (int id, ISaleService saleService) =>
                {
                    var sale = await saleService.GetSaleByIdAsync(id);
                    if (sale == null)
                        return Results.NotFound();

                    return Results.Ok(sale.MapToSaleResponse());
                }
            )
            .WithName("GetSale");

        saleGroup.MapGet(
                ApiEnpoints.Sale.GetAllSales,
                async (ISaleService saleService) =>
                {
                    var sales = await saleService.GetAllSalesAsync();
                    var salesResponse = sales.MapToSaleResponse();
                    return Results.Ok(salesResponse);
                }
            )
            .WithName("GetAllSales");

        saleGroup.MapPost(
                ApiEnpoints.Sale.ConfirmSale,
                async (int id, ISaleService saleService) =>
                {
                    try
                    {
                        var sale = await saleService.ConfirmSaleAsync(id);
                        if (sale == null)
                            return Results.NotFound();

                        return Results.Ok(sale.MapToSaleResponse());
                    }
                    catch (InvalidOperationException ex)
                    {
                        return Results.BadRequest(new { error = ex.Message });
                    }
                }
            )
            .WithName("ConfirmSale");

        saleGroup.MapPost(
                ApiEnpoints.Sale.CancelSale,
                async (int id, ISaleService saleService) =>
                {
                    var sale = await saleService.CancelSaleAsync(id);
                    if (sale == null)
                        return Results.NotFound();

                    return Results.Ok(sale.MapToSaleResponse());
                }
            )
            .WithName("CancelSale");
    }
}
