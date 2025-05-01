using Microsoft.AspNetCore.Authentication.BearerToken;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthentication(BearerTokenDefaults.AuthenticationScheme)
  .AddBearerToken();

builder.Services.AddCors(options =>
{
  options.AddPolicy("customPolicy", builder =>
  {
    builder.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
  });
});

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors();

app.MapReverseProxy();

app.Run();
