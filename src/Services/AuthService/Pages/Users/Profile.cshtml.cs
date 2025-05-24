using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IdentityServer.Models;

namespace IdentityServer.Pages.Users
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string ProfilePicture { get; set; }
            public bool IsActive { get; set; }
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmPassword { get; set; }
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                IsActive = user.IsActive
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(Input.Id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            // Update basic info
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.PhoneNumber = Input.PhoneNumber;
            user.IsActive = Input.IsActive;
            user.LastModifiedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            StatusMessage = "Profile updated successfully.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            if (string.IsNullOrEmpty(Input.CurrentPassword) || 
                string.IsNullOrEmpty(Input.NewPassword) || 
                string.IsNullOrEmpty(Input.ConfirmPassword))
            {
                ModelState.AddModelError(string.Empty, "All password fields are required.");
                return Page();
            }

            if (Input.NewPassword != Input.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "The new password and confirmation password do not match.");
                return Page();
            }

            var user = await _userManager.FindByIdAsync(Input.Id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.CurrentPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            StatusMessage = "Password changed successfully.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUploadProfilePictureAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a file.");
                return Page();
            }

            var user = await _userManager.FindByIdAsync(Input.Id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            // Save file to wwwroot/uploads/profile-pictures
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile-pictures");
            Directory.CreateDirectory(uploadsFolder); // Create if not exists

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Update user profile picture path
            user.ProfilePicture = $"/uploads/profile-pictures/{uniqueFileName}";
            user.LastModifiedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            StatusMessage = "Profile picture updated successfully.";
            return RedirectToPage();
        }
    }
}
