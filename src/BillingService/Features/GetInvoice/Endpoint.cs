using SharedKernal.CQRS;

namespace BillingService.Features.Invoices
{
  public static class GetInvoiceByIdEndpoint
  {
    public static RouteGroupBuilder MapGetInvoiceById(this RouteGroupBuilder group)
    {
      group.MapGet("/invoices/{id}", async (Guid id, ICommandHandler<GetInvoiceCommand, GetInvoiceResponse> handler) =>
      {
        var result = await handler.Handle(new GetInvoiceCommand(id));
        return Results.Ok(result);
      });

      return group;
    }
  }

}
