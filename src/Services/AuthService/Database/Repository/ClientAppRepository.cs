using IdentityServer.Records;
using OpenIddict.Abstractions;
using OpenIddict.EntityFrameworkCore.Models;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Database.Repository
{
    public class ClientAppRepository(
        IOpenIddictApplicationManager iddictManager,
        AuthDbContext context)
    {        public async Task<string> CreateClientAsync(ClientRecord client)
        {
            var clientSecret = Guid.NewGuid().ToString();
            var application = new OpenIddictApplicationDescriptor
            {
                ClientId = client.ClientId,
                ClientSecret = clientSecret,
                ApplicationType = ClientTypes.Public,
                ConsentType = ConsentTypes.Explicit,
                DisplayName = client.DisplayName,
                RedirectUris =
                {
                    new Uri(client.RedirectUri.ToString())
                },
                PostLogoutRedirectUris =
                {
                    new Uri(client.PostLogoutRedirectUris.ToString())
                },
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                   $"{Permissions.Prefixes.Scope}api1"
                },
                Requirements =
                {
                    Requirements.Features.ProofKeyForCodeExchange
                }
            };            
            
            await iddictManager.CreateAsync(application);
            return clientSecret;
        }

        public async Task<IEnumerable<ClientsSummaryRecord>> GetClientsAsync()
        {
            var clients = new List<ClientsSummaryRecord>();
            var result = iddictManager.ListAsync();
            await foreach (var item in result)
            {
                var clientApp = (OpenIddictEntityFrameworkCoreApplication)item;
                clients.Add(new ClientsSummaryRecord
                {
                    ClientId = clientApp.ClientId,
                    ClientSecret = clientApp.ClientSecret,
                    DisplayName = clientApp.DisplayName,
                    Id = clientApp.Id
                });
            }

            return clients;
        }

        public async Task<EditClientRecord> GetClientAsync(string id)
        {
            var result = (OpenIddictEntityFrameworkCoreApplication)await iddictManager.FindByIdAsync(id);

            return new EditClientRecord()
            {
                Id = result.Id,
                RedirectUri = result.RedirectUris.ToString(),
                PostLogoutRedirectUris = result.PostLogoutRedirectUris.ToString(),
                ClientId = result.ClientId,
                ClientSecret = result.ClientSecret,
                DisplayName = result.DisplayName,
            };
        }        
        
        public async Task UpdateClientAsync(EditClientRecord client)
        {
            var result = (OpenIddictEntityFrameworkCoreApplication)await iddictManager.FindByIdAsync(client.Id);

            result.ClientId = client.ClientId;
            result.ClientSecret = client.ClientSecret;
            result.DisplayName = client.DisplayName;
            result.RedirectUris = client.RedirectUri.ToString();
            result.PostLogoutRedirectUris = client.PostLogoutRedirectUris.ToString();

            await iddictManager.UpdateAsync(result);
        }

        public async Task<string> RegenerateClientSecretAsync(string id)
        {
            var application = await iddictManager.FindByIdAsync(id);
            if (application == null)
            {
                throw new InvalidOperationException("Client not found");
            }

            var newSecret = Guid.NewGuid().ToString();

            var descriptor = new OpenIddictApplicationDescriptor();
            await iddictManager.PopulateAsync(descriptor, application);
            descriptor.ClientSecret = newSecret;

            application = await iddictManager.FindByIdAsync(id);
            await iddictManager.UpdateAsync(application, descriptor);

            return newSecret;
        }
    }
}
