using AuthService.Database;
using AuthService.Extensions;
using IdentityServer.Extensions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.Extensions.Hosting;
using ServiceDefaults;

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

        if (app.Environment.IsDevelopment())
        {
            // Configure global exception handling for production
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        else
        {
            // Show developer exception page in development
            app.UseDeveloperExceptionPage();
        }

        app.ConfigurePipeline();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            db.Database.Migrate();
        }

        app.Run();
    }
}