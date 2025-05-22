using Bogus;
using ProductService.Database;
using ProductService.Database.Entities;

namespace ProductService;

public class ProductDataSeeder
{
    private readonly ProductDbContext _context;
    private readonly ILogger<ProductDataSeeder> _logger;

    public ProductDataSeeder(ProductDbContext context, ILogger<ProductDataSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            if (!_context.Products.Any())
            {
                // Feature faker definition to be used later
                var featureFaker = new Faker<Feature>()
                    .RuleFor(f => f.Name, f => f.Commerce.ProductAdjective())
                    .RuleFor(f => f.Description, f => f.Lorem.Sentence())
                    .RuleFor(f => f.CreatedAt, f => f.Date.Past());

                // Generate Products first
                var productFaker = new Faker<Product>()
                    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                    .RuleFor(p => p.CreatedAt, f => f.Date.Past())
                    .RuleFor(p => p.TenantId, f => f.Random.Guid())
                    .RuleFor(p => p.IsActive, f => f.Random.Bool(0.9f));

                var products = productFaker.Generate(15);
                await _context.Products.AddRangeAsync(products);
                await _context.SaveChangesAsync();

                // For each product, generate plans
                foreach (var product in products)
                {
                    var planCount = new Faker().Random.Number(1, 3);
                    var plans = new List<Plan>();

                    for (int i = 0; i < planCount; i++)
                    {
                        var plan = new Faker<Plan>()
                            .RuleFor(p => p.Name, f => f.Commerce.ProductMaterial())
                            .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
                            .RuleFor(p => p.AnnaulPrice, f => decimal.Parse(f.Commerce.Price(50, 500)))
                            .RuleFor(p => p.MonthlyPrice, f => decimal.Parse(f.Commerce.Price(50, 500)))
                            .RuleFor(p => p.CreatedAt, f => f.Date.Past())
                            .RuleFor(p => p.IsActive, f => f.Random.Bool(0.9f))
                            .RuleFor(p => p.ProductId, _ => product.Id)
                            .Generate();

                        // Generate features for this specific plan
                        var featureCount = new Faker().Random.Number(3, 8);
                        var features = featureFaker.Generate(featureCount);
                        plan.Features = features;

                        plans.Add(plan);
                    }

                    await _context.Plans.AddRangeAsync(plans);
                    await _context.SaveChangesAsync();
                }

                _logger.LogInformation("Seed data added successfully");
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