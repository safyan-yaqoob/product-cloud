using BillingService.External.Models;
using System.Net;
using System.Net.Http;

namespace BillingService.External
{
  public class SubscriptionServiceClient(HttpClient httpClient)
  {
    public async Task<SubscriptionDto?> GetActiveSubscriptionAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
      var response = await httpClient.GetAsync($"/api/subscriptions/{tenantId}", cancellationToken);

      if (response.StatusCode == HttpStatusCode.NotFound)
        return null;

      response.EnsureSuccessStatusCode();
      return await response.Content.ReadFromJsonAsync<SubscriptionDto>(cancellationToken: cancellationToken);
    }
  }
}
