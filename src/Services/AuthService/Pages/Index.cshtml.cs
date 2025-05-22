using AuthService.Database.Repository;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ClientAppRepository _clientRepository;

        public int TotalUsers { get; private set; }
        public int TotalClients { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, UserManager<ApplicationUser> userManager, ClientAppRepository clientRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _clientRepository = clientRepository;
        }

        public async Task OnGetAsync()
        {
            TotalUsers = _userManager.Users.Count();
            TotalClients = (await _clientRepository.GetClientsAsync()).Count();
        }
    }
}
