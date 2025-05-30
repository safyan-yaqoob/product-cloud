﻿using System.Security.Claims;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Client.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Controllers
{
    [ApiController]
    public class ExternalCallbackController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
        : Controller
    {
        [HttpPost("~/signin-google"), HttpGet("~/signin-google"), IgnoreAntiforgeryToken]
        public async Task<ActionResult> LogInCallback(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            return await ProcessSuccessfulExternalLogin(returnUrl);
        }

        private async Task<ActionResult> ProcessSuccessfulExternalLogin(string returnUrl)
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                return RedirectToLoginPage(returnUrl);
            }

            if (!authenticateResult.Principal?.Identity?.IsAuthenticated ?? false)
            {
                throw new InvalidOperationException("The external authorization data cannot be used for authentication.");
            }

            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                throw new InvalidOperationException("Email claim not found.");
            }

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = CreateUser();
                user.Email = email;
                user.UserName = email;
                
                var userCreateResult = await userManager.CreateAsync(user);
                if (!userCreateResult.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to create user: {string.Join(", ", userCreateResult.Errors.Select(error => error.Description))}");
                }
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Email, email));
            identity.AddClaim(new Claim(ClaimTypes.Name, authenticateResult.Principal.FindFirst(ClaimTypes.Name)?.Value ?? email));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.Id.ToString()));

            if (authenticateResult.Properties.Items.TryGetValue("registration_id", out var registrationId))
            {
                identity.AddClaim(new Claim("registration_id", registrationId));
            }

            var properties = new AuthenticationProperties(authenticateResult.Properties.Items)
            {
                RedirectUri = authenticateResult.Properties.RedirectUri ?? "/"
            };

            var tokensToStore = authenticateResult.Properties.GetTokens().Where(token =>
                token.Name is OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken or
                OpenIddictClientAspNetCoreConstants.Tokens.BackchannelIdentityToken or
                OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken);

            properties.StoreTokens(tokensToStore);

            await signInManager.SignInAsync(user, false, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

            return LocalRedirect(returnUrl);
        }

        private ActionResult RedirectToLoginPage(string returnUrl)
        {
            return RedirectToPage("/Login/Index", new { ReturnUrl = returnUrl });
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }
    }
}
