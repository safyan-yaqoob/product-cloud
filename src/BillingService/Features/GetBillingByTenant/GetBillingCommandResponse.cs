namespace BillingService.Features.GetBillingByTenant
{
  public record GetBillingCommandResponse
  {
    public InvoiceDto? LatestInvoice { get; set; }
    public List<BillingTransactionDto> RecentTransactions { get; set; } = [];
  }

  public record InvoiceDto(
    Guid Id,
    decimal Amount,
    DateTime IssueDate,
    DateTime DueDate,
    string Status,
    string InvoiceNumber);

  public record BillingTransactionDto(
      Guid Id,
      decimal Amount,
      string Currency,
      string Status,
      DateTime CreatedAt,
      DateTime? PaidAt);
}
