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
                .SetClaims(Claims.Role, new List<string> { "user", "admin" }.ToImmutableArray());

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
					properties: new AuthenticationProperties(new Dictionary<string, string?>
					{
						[OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
						[OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
							"Cannot find user from the token."
					}));
			}

			var user = await userManager.FindByEmailAsync(email);

			var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType, Claims.Name, Claims.Role);

			// Add claims and set destinations
			var subjectClaim = new Claim(Claims.Subject, email);
			subjectClaim.SetDestinations(Destinations.AccessToken, Destinations.IdentityToken);
			identity.AddClaim(subjectClaim);

			var emailClaim = new Claim(Claims.Email, email);
			emailClaim.SetDestinations(Destinations.AccessToken, Destinations.IdentityToken);
			identity.AddClaim(emailClaim);

			var nameClaim = new Claim(Claims.Name, $"{user.FirstName} {user.LastName}");
			nameClaim.SetDestinations(Destinations.AccessToken, Destinations.IdentityToken);
			identity.AddClaim(nameClaim);

			var userIdClaim = new Claim("userId", user.Id.ToString());
			userIdClaim.SetDestinations(Destinations.AccessToken, Destinations.IdentityToken);
			identity.AddClaim(userIdClaim);

			var roleClaim = new Claim(Claims.Role, "admin");
			roleClaim.SetDestinations(Destinations.AccessToken, Destinations.IdentityToken);
			identity.AddClaim(roleClaim);

			identity.SetScopes(request.GetScopes());
			identity.SetResources(await scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());

			return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
		}

		[HttpGet("~/connect/logout")]
        [HttpPost("~/connect/logout")]
        public async Task<IActionResult> LogoutPost()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return SignOut(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = "/"
                });
        }
    }
}
