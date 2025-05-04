using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using IdentityServer.Data;
using IdentityServer.Data.Repository;
using IdentityServer.Models;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace IdentityServer.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
            {
                o.SignIn.RequireConfirmedAccount = !true;
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
              options.UseEntityFrameworkCore()
                      .UseDbContext<AuthDbContext>();
            })
                .AddServer(options =>
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

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(c =>
            {
              c.LoginPath = "/Identity/Account/Login";
            });

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
