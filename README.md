# SaaS Marketplace ASP.NET Core API Template With (Microservices, Vertical Slice Architecture, .NET Aspire)

[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://codespaces.new/safyan-yaqoob/product-cloud)

> This is an **multi-tenant** SaaS marketplace boilerplate built with **ASP.NET Core** `template` based on `microservices` architecture. Perfect starter for creating your own SaaS marketplace. You are more than welcome to use this `template` and modify it based on your requirements.

## ⭐ Support

If you like it or it helped you lear, please give ⭐ to this repository.
Thanks!

## 📖 Table of Contents

-   [Microservices Template with vertical slice architecture](#mircoservices-template)
    -   [Install](#setup)
    -   [Features](#features)
    -   [Libraries](#libraries)
    -   [Getting Started](#getting-started)
        -   [Dev Certificate](#dev-certificate)
    -   [Application Structure](#application-structure)
        -   [Folder Structure](#folder-structure)
    -   [License](#license)

---

## Setup
* Clone this repository
```bash
git clone https://github.com/safyan-yaqoob/product-cloud.git
```

## Getting Started

1. This application uses `HTTPS` for hosting APIs, to set up a valid certificate on your machine, you can create a [Self-Signed Certificate](https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-9.0#macos-or-linux), see more about enforce certificate [here](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl).
2. Install git - [https://git-scm.com/downloads](https://git-scm.com/downloads).
3. Install .NET Core 9.0 - [https://dotnet.microsoft.com/download/dotnet/9.0](https://dotnet.microsoft.com/download/dotnet/9.0).
4. Install Visual Studio, Rider, or VSCode.
11. Swagger UI for API documentation in each service
12. [Docker](https://www.docker.com/) (for Aspire orchestration)
13. Run with .NET Aspire
    
```bash
dotnet aspire run
```

## Features
-   ✅ Using `Microservices Architecture` as a high-level architecture.
-   ✅ Using `Vertical Slice Architecture` inside each service except `Auth Service`.
-   ✅ Multi-Tenancy: Isolated tenant stores with customizable branding (logo, colors, timezone).
-   ✅ Service(**Auth Service**): Authentication and authorization service (`OpenIddict.NET`).
-   ✅ Service(**Tenant Service**): Organization onboarding & settings
-   ✅ Service(**Product Service**): APIs for configure products & plans.
-   ✅ Service(**Subscription Service**): Dealing with subscription related APIs.
-   ✅ Service(**Billing Service**): Dealing with payments, Stripe integration, pre-checkout intents.
-   ✅ API Gateway: YARP for routing, load balancing, and hot reload.
-   ✅ Using Docker and `docker-compose` for deployment.
---

## Libraries

-   ✔️ **[`.NET 9`](https://dotnet.microsoft.com/download)** - .NET Framework and .NET Core, including ASP.NET and ASP.NET Core
-   ✔️ **[`.NET Aspire`](https://github.com/dotnet/aspire)** - .NET Aspire for cloud-ready orchestration and tooling related to open telemetry.
-   ✔️ **[`Npgsql Entity Framework Core Provider`](https://www.nuget.org/packages/Aspire.Npgsql.EntityFrameworkCore.PostgreSQL)** - Npgsql has an Entity Framework (EF) Core provider.
-   ✔️ **[`Serilog`](https://github.com/serilog/serilog)** - Simple .NET logging with fully-structured events (Congfigure only for the `auth-service` yet).
-   ✔️ **[`Polly`](https://github.com/App-vNext/Polly)** - Polly is a .NET resilience and transient-fault-handling library that allows developers to express policies such as Retry, Circuit Breaker, Timeout, Bulkhead Isolation, and Fallback in a fluent and thread-safe manner
-   ✔️ **[`Scrutor`](https://github.com/khellang/Scrutor)** - Assembly scanning and decoration extensions for Microsoft.Extensions.DependencyInjection
-   ✔️ **[`Opentelemetry-dotnet`](https://github.com/open-telemetry/opentelemetry-dotnet)** - The OpenTelemetry .NET Client
-   ✔️ **[`Newtonsoft.Json`](https://github.com/JamesNK/Newtonsoft.Json)** - Json.NET is a popular high-performance JSON framework for .NET
-   ✔️ **[`AspNetCore.Diagnostics.HealthChecks`](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks)** - Enterprise HealthChecks for ASP.NET Core Diagnostics Package

### Folder Structure

```cmd
Solution Items
│   .editorconfig
│   Directory.Build.props
│   Directory.Packages.props
src
│   ├───Aspire
│   │   ├───ProductCloud.Host.csproj
│   │   ├───ProductCloud.ServiceDefaults.csproj
│   ├───BuildingBlocks
│   │   ├───ProductCloud.SharedKernel.csproj
│   │   │   ├───CQRS
│   │   │   ├───Caching
│   │   │   ├───CQRS
│   │   │   ├───Common
│   │   │   ├───Infrastructure
│   │   │   ├───Messaging (Message Broker)
│   │   │   ├───Protos (Grpc)
│   ├───Services
│   │   ├───AuthService
│   │   |   |───AuthService.csproj (ASP.NET Core Razor Pages)
│   │   |   |───Dockerfile
│   │   ├───BillingService
│   │   |   |───BillingSerivce.csproj (ASP.NET Core API)
│   │   |   |───Dockerfile
│   │   ├───ProductService
│   │   |   |───ProductService.csproj (ASP.NET Core API)
│   │   |   |───Dockerfile
│   │   ├───SubscriptionService
│   │   |   |───SubscriptionService.csproj (ASP.NET Core API)
│   │   |   |───Dockerfile
│   │   ├───TenantService
│   │   |   |───TenantService.csproj (ASP.NET Core API)
│   │   |   |───Dockerfile
│   ├───Api Gateway
│   │   ├───api.gateway.Host.csproj
docker-compose
│   ├───docker-compose.yml
```


## 📜 License

Distributed under the MIT License. See [LICENSE.md](LICENSE.md) for details.
