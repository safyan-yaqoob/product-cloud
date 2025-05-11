var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis")
	.WithLifetime(ContainerLifetime.Persistent);

var rabbitMq = builder.AddRabbitMQ("product-cloud-mq")
    .WithManagementPlugin();

var postgres = builder.AddPostgres("postgres")
	.WithPgAdmin()
	.WithDataVolume()
	.WithLifetime(ContainerLifetime.Persistent);

var tenantDb = postgres.AddDatabase("tenantDb");
var billingDb = postgres.AddDatabase("billingDb");
var subscriptionDb = postgres.AddDatabase("subscriptionDb");
var productDb = postgres.AddDatabase("productDb");
var authDb = postgres.AddDatabase("authDb");

var tenantService = builder.AddProject("tenant-service", "../../src/TenantService/TenantService.csproj")
	.WithReference(tenantDb)
	.WithReference(rabbitMq)
	.WithReference(redis)
	.WaitFor(tenantDb);

var billingService = builder.AddProject("billing-service", "../../src/BillingService/BillingService.csproj")
	//.WithHttpEndpoint(name: "billing-http", port: 5002)
	.WithReference(billingDb)
    .WithReference(rabbitMq)
    .WithReference(redis)
	.WaitFor(billingDb)
    .WaitFor(rabbitMq);

var subscriptionService = builder.AddProject("subscription-service", "../../src/SubscriptionService/SubscriptionService.csproj")
	//.WithHttpEndpoint(name: "subscription-http", port: 5003)
	.WithReference(subscriptionDb)
    .WithReference(rabbitMq)
    .WithReference(redis)
	.WaitFor(subscriptionDb)
    .WaitFor(rabbitMq);

var productService = builder.AddProject("product-service", "../../src/ProductService/ProductService.csproj")
	//.WithHttpEndpoint(name: "product-http", port: 5004)
	.WithReference(productDb)
    .WithReference(rabbitMq)
    .WithReference(redis)
	.WaitFor(productDb)
    .WaitFor(rabbitMq);

var authService = builder.AddProject("auth-service", "../../src/AuthService/AuthService.csproj")
	//.WithHttpEndpoint(name: "auth-http", port: 5005)
	.WithReference(authDb)
    .WithReference(rabbitMq)
    .WithReference(redis)
	.WaitFor(authDb)
    .WaitFor(rabbitMq);

var gateway = builder.AddProject("api-gateway", "../../api.gateway/api.gateway.csproj")
	.WithReference(tenantService)
	.WithReference(billingService)
	.WithReference(subscriptionService)
	.WithReference(productService)
	.WithReference(authService);


builder.Build().Run();
