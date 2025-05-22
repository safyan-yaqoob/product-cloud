using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthService.Middleware;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string[] _allowedPaths;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;        _allowedPaths = new[]
        {
            "/Identity/Account/Login",
            "/Identity/Account/Register",
            "/Identity/Account/ExternalLogin",
            "/Identity/Account/ForgotPassword",
            "/Identity/Account/ResetPassword",
            "/signin-google",
            "/connect/authorize",
            "/connect/token",
            "/connect/userinfo",
            "/css",
            "/js",
            "/lib"
        };
    }    
    
    public async Task InvokeAsync(HttpContext context)
    {
        // Skip authentication check for allowed paths
        if (IsAllowedPath(context.Request.Path))
        {
            await _next(context);
            return;
        }

        // Check if user is authenticated
        if (!context.User.Identity?.IsAuthenticated == true)
        {
            // Don't redirect if we're already on the login page (prevents redirect loop)
            if (context.Request.Path.StartsWithSegments("/Identity/Account/Login"))
            {
                await _next(context);
                return;
            }

            // Store the original requested URL
            var returnUrl = context.Request.Path + context.Request.QueryString;
            
            // Redirect to login page with return URL
            context.Response.Redirect($"/Identity/Account/Login?ReturnUrl={Uri.EscapeDataString(returnUrl)}");
            return;
        }

        await _next(context);
    }

    private bool IsAllowedPath(string path)
    {
        return _allowedPaths.Any(allowedPath => 
            path.Equals(allowedPath, StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith(allowedPath + "/", StringComparison.OrdinalIgnoreCase));
    }
}
