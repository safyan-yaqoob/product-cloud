var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
	.WithPgAdmin()
	.WithLifetime(ContainerLifetime.Persistent);

if (builder.ExecutionContext.IsRunMode)
{
	postgres.WithDataVolume();
}

var tenantDb = postgres.AddDatabase("tenantDb");
var billingDb = postgres.AddDatabase("billingDb");
var subscriptionDb = postgres.AddDatabase("subscriptionDb");
var productDb = postgres.AddDatabase("productDb");
var authDb = postgres.AddDatabase("authDb");

var dbManager = builder.AddProject("dbmanager", "../../src/DbManager/DbManager.csproj")
	.WithReference(tenantDb)
	.WaitFor(tenantDb)
	.WithReference(billingDb)
	.WaitFor(billingDb)
	.WithReference(subscriptionDb)
	.WaitFor(subscriptionDb)
	.WithReference(productDb)
	.WaitFor(productDb)
	.WithReference(authDb)
	.WaitFor(authDb)
	.WithHttpHealthCheck("/health")
	.WithHttpCommand("/reset-db", "Reset Database", commandOptions: new() { IconName = "DatabaseLightning" });

var tenantService = builder.AddProject("tenant-service", "../../src/TenantService/TenantService.csproj")
	.WithReference(tenantDb)
	.WaitFor(dbManager);

var billingService = builder.AddProject("billing-service", "../../src/BillingService/BillingService.csproj")
	.WithHttpEndpoint(name: "billing-http", port: 5002)
	.WithReference(billingDb)
	.WaitFor(dbManager);

var subscriptionService = builder.AddProject("subscription-service", "../../src/SubscriptionService/SubscriptionService.csproj")
	.WithHttpEndpoint(name: "subscription-http", port: 5003)
	.WithReference(subscriptionDb)
	.WaitFor(dbManager);

var productService = builder.AddProject("product-service", "../../src/ProductService/ProductService.csproj")
	.WithHttpEndpoint(name: "product-http", port: 5004)
	.WithReference(productDb)
	.WaitFor(dbManager);

var authService = builder.AddProject("auth-service", "../../src/AuthService/AuthService.csproj")
	.WithHttpEndpoint(name: "auth-http", port: 5005)
	.WithReference(authDb)
	.WaitFor(dbManager);

var gateway = builder.AddProject("api-gateway", "../../api.gateway/api.gateway.csproj")
	.WithReference(tenantService)
	.WithReference(billingService)
	.WithReference(subscriptionService)
	.WithReference(productService)
	.WithReference(authService);

builder.Build().Run();
