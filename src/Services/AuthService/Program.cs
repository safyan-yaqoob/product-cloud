using AuthService.Database;
using AuthService.Extensions;
using IdentityServer.Extensions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        builder.AddNpgsqlDbContext<AuthDbContext>("authDb", null, options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("authDb"),
                npgsql => npgsql.MigrationsAssembly(typeof(AuthDbContext).Assembly.GetName().Name));

            options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
                .EnableDetailedErrors();

            options.UseOpenIddict();
        });

        builder.Services.RegisterServices(builder.Configuration);

        var app = builder.Build();

        app.ConfigurePipeline();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            db.Database.Migrate(); // Apply any pending EF Core migrations
        }
        app.Run();
    }
}