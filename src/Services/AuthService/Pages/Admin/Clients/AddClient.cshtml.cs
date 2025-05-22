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
        }        [BindProperty]
        public ClientRecord InputModel { get; set; }

        public bool Created { get; set; }

        public void OnGet()
        {
            InputModel = new ClientRecord();
        }        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                InputModel.ClientSecret = await _repository.CreateClientAsync(InputModel);
                Created = true;
            }

            return Page();
        }
    }
}
