using BillingService.External.Models;
using System.Net;

namespace BillingService.External
{
  public class TenantServiceClient(HttpClient httpClient)
  {
    public async Task<TenantDto?> GetTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
      var response = await httpClient.GetAsync($"/api/tenants/{tenantId}", cancellationToken);

      if (response.StatusCode == HttpStatusCode.NotFound)
        return null;

      response.EnsureSuccessStatusCode();
      return await response.Content.ReadFromJsonAsync<TenantDto>(cancellationToken: cancellationToken);
    }
  }
}
