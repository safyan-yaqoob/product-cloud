using Bogus;
using TenantService.Common;
using TenantService.Database;
using TenantService.Database.Entities;

namespace TenantService;

public class TenantDataSeeder
{
    private readonly TenantDbContext _context;
    private readonly ILogger<TenantDataSeeder> _logger;

    public TenantDataSeeder(TenantDbContext context, ILogger<TenantDataSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            if (!_context.Tenants.Any())
            {
                // Define industries for realistic data
                var industries = new[] { "Technology", "Healthcare", "Finance", "Education", "Manufacturing", "Retail" };
                var timeZones = TimeZoneInfo.GetSystemTimeZones().Select(tz => tz.Id).ToArray();

                var tenantFaker = new Faker<Tenant>()
                    .CustomInstantiator(f => Tenant.Create(
                        name: f.Company.CompanyName(),
                        orgnization: f.Company.CompanySuffix(),
                        contactEmail: f.Internet.Email(),
                        plan: f.Random.Number(1, 4), // Assuming 4 plan types
                        industry: f.PickRandom(industries),
                        logo: f.Internet.Avatar(),
                        timeZone: f.PickRandom(timeZones),
                        userId: Guid.NewGuid()
                    ));

                var tenants = tenantFaker.Generate(10);

                // Simulate some tenants with different statuses
                tenants.Take(2).ToList().ForEach(t => t.ChangeStatus(TenantStatus.Suspended));
                tenants.Skip(8).Take(1).ToList().ForEach(t => t.ChangeStatus(TenantStatus.Active));

                // Add some plan expiration dates
                var random = new Random();
                tenants.ForEach(t => 
                {
                    var daysToAdd = random.Next(30, 365);
                    typeof(Tenant).GetProperty("PlanExpiresAt")!
                        .SetValue(t, DateTime.UtcNow.AddDays(daysToAdd));
                });

                await _context.Tenants.AddRangeAsync(tenants);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Seed data added successfully: {Count} tenants created", tenants.Count);
            }
            else
            {
                _logger.LogInformation("Data already exists - skipping seed");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding data");
            throw;
        }
    }
}