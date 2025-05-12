using ProductCloud.SharedKernal.CQRS;

namespace BillingService.Features.GetInvoicesByTenant
{
    public static class GetInvoicesByTenantEndpoint
    {
        public static RouteGroupBuilder MapGetInvoicesByTenant(this RouteGroupBuilder group)
        {
            group.MapGet("/invoices/{tenantId}", async (Guid tenantId, ICommandHandler<GetInvoicesByTenantCommand, IEnumerable<GetInvoicesResponse>> handler) =>
            {
                var result = await handler.Handle(new GetInvoicesByTenantCommand(tenantId));
                return Results.Ok(result);
            });

            return group;
        }
    }

}
