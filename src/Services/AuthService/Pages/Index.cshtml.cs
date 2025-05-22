using AuthService.Database.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IdentityServer.Models;

namespace IdentityServer.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ClientAppRepository _clientRepository;
        private readonly ScopesRepository _scopesRepository;

        public int UsersCount { get; set; }
        public int ClientsCount { get; set; }
        public int ScopesCount { get; set; }

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            ClientAppRepository clientRepository,
            ScopesRepository scopesRepository)
        {
            _userManager = userManager;
            _clientRepository = clientRepository;
            _scopesRepository = scopesRepository;
        }

        public async Task OnGetAsync()
        {
            UsersCount = await _userManager.Users.CountAsync();
            ClientsCount = (await _clientRepository.GetClientsAsync()).Count();
            ScopesCount = (await _scopesRepository.GetScopesAsync()).Count();
        }
    }
}
