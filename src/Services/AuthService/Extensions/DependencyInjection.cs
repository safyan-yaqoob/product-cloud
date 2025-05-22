using AuthService.Database;
using AuthService.Database.Repository;
using AuthService.Services;
using IdentityServer;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using ProductCloud.SharedKernal.Infrastructure;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSharedInfrastructure(configuration);

            // Configure EmailSender Options
            services.Configure<EmailSenderOptions>(configuration.GetSection("EmailSender"));
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
            {
                o.SignIn.RequireConfirmedAccount = true; // Enable email confirmation
                o.Password = new PasswordOptions
                {
                    RequireDigit = false,
                    RequiredLength = 1,
                    RequiredUniqueChars = 0,
                    RequireLowercase = false,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false
                };
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AuthDbContext>();            
            
            services.AddOpenIddict().AddCore(options =>
            {
                options.UseEntityFrameworkCore().UseDbContext<AuthDbContext>();
            }).AddServer(options =>
            {
                options.SetAuthorizationEndpointUris("connect/authorize")
                            .SetTokenEndpointUris("connect/token");

                options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

                options.AllowAuthorizationCodeFlow();

                options.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();

                options.UseAspNetCore()
                        .EnableAuthorizationEndpointPassthrough()
                        .EnableTokenEndpointPassthrough();

                options.DisableAccessTokenEncryption();

            }).AddClient(options =>
            {
                options.AllowAuthorizationCodeFlow();
                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                options.UseAspNetCore()
                       .EnableRedirectionEndpointPassthrough();

                options.UseSystemNetHttp();

                // Register the Google integration.
                options.UseWebProviders().AddGoogle(options =>
                {
                    options.SetClientId("client_id")
                    .SetClientSecret("client_secrets")
                    .SetRedirectUri("/signin-google")
                    .SetProviderDisplayName("Sign In With Google")
                    .AddScopes("email profile");
                });

            }).AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });

            services.AddControllers();
            services.AddRazorPages();            
            
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.Cookie.Name = "auth_cookie";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.ExpireTimeSpan = TimeSpan.FromHours(12);
                options.SlidingExpiration = true;
            });

            services.AddAuthorizationBuilder()
                .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build());

            services.AddTransient<AuthorizationService>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("https://localhost:7002")
                        .AllowAnyHeader();

                    policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader();
                });
            });

            services.AddTransient<ClientAppRepository>();
            services.AddTransient<ScopesRepository>();
            services.AddScoped<IdentityDataSeeder>();

            return services;
        }
    }
}
