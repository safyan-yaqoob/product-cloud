using AuthService.Database.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IdentityServer.Records;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Pages.Clients
{    
    public class IndexModel : PageModel
    {
        private readonly ClientAppRepository _repository;

        public IndexModel(ClientAppRepository repository)
        {
            _repository = repository;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IEnumerable<ClientsSummaryRecord> Clients { get; private set; } = default!;
        
        public async Task OnGetAsync()
        {
            Clients = await _repository.GetClientsAsync();
        }

        public async Task<IActionResult> OnPostRegenerateSecretAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            try
            {
                var newSecret = await _repository.RegenerateClientSecretAsync(id);
                TempData["StatusMessage"] = $"New client secret: {newSecret}";
            }
            catch (Exception ex)
            {
                TempData["StatusMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
