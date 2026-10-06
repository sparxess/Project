using DirectoryService.Api;
using DirectoryService.Api.Middlewares;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProgramServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionMiddleware();

app.MapControllers();
app.MapHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

await app.RunAsync();
