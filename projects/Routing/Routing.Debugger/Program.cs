using Routing.Debugger.Middlewares;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<RouteDebuggerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/test/{id}", (int id) =>
{
    return $"Test with id = {id}";
})
.WithName("Test");

app.Run();