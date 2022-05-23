using BuildingBlocks.Abstractions.Web;

namespace ECommerce.Modules.Identity.Identity.Features.VerifyEmail;

public static class VerifyEmailEndpoint
{
    internal static IEndpointRouteBuilder MapSendVerifyEmailEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                $"{IdentityConfigs.IdentityPrefixUri}/verify-email", VerifyEmail)
            .WithTags(IdentityConfigs.Tag)
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDisplayName("Verify Email.");

        return endpoints;
    }

    private static Task<IResult> VerifyEmail(
        VerifyEmailRequest request,
        IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var command = new VerifyEmailCommand(request.Email, request.Code);

            await commandProcessor.SendAsync(command, cancellationToken);

            return Results.Ok();
        });
    }
}
