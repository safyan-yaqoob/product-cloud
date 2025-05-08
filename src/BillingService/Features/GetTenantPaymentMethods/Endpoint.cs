using SharedKernal.CQRS;

namespace BillingService.Features.GetTenantPaymentMethods
{
    public static class GetTenantEndpoint
    {
        public static RouteGroupBuilder MapGetTenantPaymentMethods(this RouteGroupBuilder group)
        {
            group.MapGet("/methods/{tenantId}", async (Guid tenantId, ICommandHandler<GetPaymentMethodCommand, GetPaymentMethodCommandResponse> handler) =>
            {
                var result = await handler.Handle(new GetPaymentMethodCommand(tenantId));
                return Results.Ok(result);
            });

            return group;
        }
    }
}
