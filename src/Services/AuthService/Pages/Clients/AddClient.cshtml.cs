using AuthService.Database.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IdentityServer.Records;

namespace IdentityServer.Pages.Clients
{
    public class AddClient : PageModel
    {
        private readonly ClientAppRepository _repository;

        public AddClient(ClientAppRepository repository)
        {
            _repository = repository;
        }        
        
        [BindProperty]
        public ClientRecord InputModel { get; set; }

        public bool Created { get; set; }

        public void OnGet()
        {
            InputModel = new ClientRecord();
        }        
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var clientApp = await _repository.CreateClientAsync(InputModel);
                if(clientApp == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create client application.");
                    return Page();
                }

                if(Guid.TryParse(clientApp.Id, out var id))
                {
                    InputModel.Id = id;
                }

                InputModel.ClientSecret = clientApp.ClientSecret;
                InputModel.ClientId = clientApp.ClientId;
                InputModel.DisplayName = clientApp.DisplayName;
                InputModel.RedirectUri = clientApp.RedirectUris.ToString();
                InputModel.PostLogoutRedirectUris = clientApp.PostLogoutRedirectUris.ToString();
                Created = true;
            }

            return Page();
        }
    }
}
