using System.Collections.Immutable;
using System.Security.Claims;
using IdentityServer;
using IdentityServer.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Controllers
{
    [ApiController]
    public class AuthorizationController(
	    IOpenIddictApplicationManager applicationManager,
	    IOpenIddictScopeManager scopeManager,
	    AuthorizationService authService,
	    IOpenIddictAuthorizationManager authorizationManager,
	    UserManager<ApplicationUser> userManager) : Controller
    {
	    private readonly IOpenIddictAuthorizationManager _authorizationManager = authorizationManager;

	    [HttpGet("~/connect/authorize")]
        [HttpPost("~/connect/authorize")]
        public async Task<IActionResult> Authorize()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            var parameters = authService.ParseOAuthParameters(HttpContext, new List<string> { Parameters.Prompt });

            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authService.IsAuthenticated(result, request))
            {
                return Challenge(properties: new AuthenticationProperties
                {
                    RedirectUri = authService.BuildRedirectUrl(HttpContext.Request, parameters)
                }, new[] { CookieAuthenticationDefaults.AuthenticationScheme });
            }

            var application = await applicationManager.FindByClientIdAsync(request.ClientId!) ??
                              throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

            var userId = result.Principal!.FindFirst(ClaimTypes.Email)!.Value;

            var identity = new ClaimsIdentity(
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role);

            identity.SetClaim(Claims.Subject, userId)
                .SetClaim(Claims.Email, userId)
                .SetClaim(Claims.Name, userId)
                .SetClaims(Claims.Role, new List<string> { "user" }.ToImmutableArray());

            identity.SetScopes(request.GetScopes());
            identity.SetResources(await scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
            identity.SetDestinations(c => AuthorizationService.GetDestinations(identity, c));

            return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

		[HttpPost("~/connect/token")]
		public async Task<IActionResult> Exchange()
		{
			var request = HttpContext.GetOpenIddictServerRequest() ??
						  throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

			if (!request.IsAuthorizationCodeGrantType() &&
				!request.IsRefreshTokenGrantType())
				throw new InvalidOperationException("The specified grant type is not supported.");

			var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

			var email = result.Principal.GetClaim(Claims.Subject);

			if (string.IsNullOrEmpty(email))
			{
				return Forbid(
					authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
					properties: new AuthenticationProperties(new Dictionary<string, string>
					{
						[OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
						[OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
							"Cannot find user from the token."
					}));
			}

			var user = await userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return Forbid(
					authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
					properties: new AuthenticationProperties(new Dictionary<string, string>
					{
						[OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
						[OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
							"User not found."
					}));
			}

			var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType, Claims.Name, Claims.Role);

			// Add the subject claim
			var subjectClaim = new Claim(Claims.Subject, user.Id.ToString());
			subjectClaim.SetDestinations(Destinations.AccessToken, Destinations.IdentityToken);
			identity.AddClaim(subjectClaim);

			// Add the email claim
			var emailClaim = new Claim(Claims.Email, email);
			emailClaim.SetDestinations(Destinations.AccessToken, Destinations.IdentityToken);
			identity.AddClaim(emailClaim);

			// Add the name claim
			var nameClaim = new Claim(Claims.Name, user.UserName);
			nameClaim.SetDestinations(Destinations.AccessToken, Destinations.IdentityToken);
			identity.AddClaim(nameClaim);

			// Add the role claims
			var roles = await userManager.GetRolesAsync(user);
			foreach (var role in roles)
			{
				var roleClaim = new Claim(Claims.Role, role);
				roleClaim.SetDestinations(Destinations.AccessToken, Destinations.IdentityToken);
				identity.AddClaim(roleClaim);
			}

            var scopes = request.GetScopes();

            identity.SetScopes(scopes);
            var resources = await scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync();

            identity.SetResources(resources);

			return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
		}

		[HttpGet("~/connect/logout")]
        [HttpPost("~/connect/logout")]
        public async Task<IActionResult> LogoutPost()
        {
            // Clear the local authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to home page after logout
            return SignOut(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = "/"
                });
        }
    }
}
