using BuildingBlocks.Abstractions.Web;

namespace ECommerce.Modules.Catalogs.Products.Features.ReplenishingProductStock;

// POST api/v1/catalog/products/{productId}/replenish-stock
public static class ReplenishingProductStockEndpoint
{
    internal static IEndpointRouteBuilder MapReplenishProductStockEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                $"{ProductsConfigs.ProductsPrefixUri}/{{productId}}/replenish-stock",
                ReplenishProductStock)
            .RequireAuthorization()
            .WithTags(ProductsConfigs.Tag)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("ReplenishProductStock")
            .WithDisplayName("Replenish product stock");

        return endpoints;
    }

    private static Task<IResult> ReplenishProductStock(
        long productId,
        int quantity,
        IGatewayProcessor<CatalogModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            await commandProcessor.SendAsync(new ReplenishingProductStock(productId, quantity), cancellationToken);

            return Results.NoContent();
        });
    }
}
