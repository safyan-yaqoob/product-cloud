using AuthService.Database.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IdentityServer.Records;

namespace IdentityServer.Pages.Scopes
{
    public class IndexModel : PageModel
    {
        private readonly ScopesRepository _repository;

        public IndexModel(ScopesRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<ScopeSummaryRecord> Scopes { get; private set; } = default!;
        public string Filter { get; set; }

        public async Task OnGetAsync(string filter)
        {
            Filter = filter;
            Scopes = await _repository.GetScopesAsync();
        }
    }
}
