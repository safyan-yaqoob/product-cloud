using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IdentityServer.Models;

namespace IdentityServer.Pages.Users
{
    public class UsersModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }

    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public IEnumerable<UsersModel> Users { get; private set; } = default!;
        public async Task<IActionResult> OnGetAsync()
        {
            Users = await _userManager.Users.Select(e => new UsersModel
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                FullName = e.FullName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                ProfilePicture = e.ProfilePicture,
                IsActive = e.IsActive,
                EmailConfirmed = e.EmailConfirmed,
                CreatedAt = e.CreatedAt,
                LastModifiedAt = e.LastModifiedAt
            }).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostToggleActiveAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = !user.IsActive;
            user.LastModifiedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);
            return RedirectToPage();
        }
    }
}
