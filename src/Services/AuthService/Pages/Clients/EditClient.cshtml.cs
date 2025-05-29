using AuthService.Database.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IdentityServer.Records;

namespace IdentityServer.Pages.Clients
{
    public class EditClient : PageModel
    {
        private readonly ClientAppRepository _repository;
        public EditClient(ClientAppRepository repository)
        {
            _repository = repository;
        }        
        
        [BindProperty]
        public EditClientRecord InputModel { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("/Admin/Clients/Index");
            }

            var model = await _repository.GetClientAsync(id);

            if (model == null)
            {
                ErrorMessage = "Client not found.";
                return RedirectToPage("/Admin/Clients/Index");
            }

            InputModel = model;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (ModelState.IsValid)
            {
                await _repository.UpdateClientAsync(InputModel);
                return RedirectToPage("/Admin/Clients/Index");
            }

            return Page();
        }
    }
}
