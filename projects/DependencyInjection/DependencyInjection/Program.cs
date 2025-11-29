using DependencyInjection.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// По умолчанию проверка области включена только в окружении разработки
builder.Host.UseDefaultServiceProvider(o =>
{
    // Отключаем проверку, чтобы посмотреть на ошибку "SingletonWrapperWithScopedData"
    o.ValidateScopes = false;  // Если использовать true, то проверка будет проводиться на всех контурах,
    o.ValidateOnBuild = false; // что будет влиять на производительность
});

builder.Services.AddOpenApi();

builder.Services.AddTransient<TransientWrapper>();
builder.Services.AddTransient<TransientData>();

builder.Services.AddScoped<ScopedWrapper>();
builder.Services.AddScoped<ScopedData>();

builder.Services.AddSingleton<SingletonWrapper>();
builder.Services.AddSingleton<SingletonData>();

builder.Services.AddSingleton<SingletonWrapperWithScopedData>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/test-transient", (TransientWrapper wrapper, TransientData data) =>
{
    return PrintInfo(wrapper, data);
});

app.MapGet("/test-scoped", (ScopedWrapper wrapper, ScopedData data) =>
{
    return PrintInfo(wrapper, data);
});

app.MapGet("/test-singleton", (SingletonWrapper wrapper, SingletonData data) =>
{
    return PrintInfo(wrapper, data);
});

app.MapGet("/test-with-dependency-capture-error",
    (SingletonWrapperWithScopedData wrapper, ScopedData data) =>
    {
        return PrintInfo(wrapper, data);
    });

app.Run();

return;

static string PrintInfo(WrapperBase wrapper, DataBase data)
{
    var wrapperCount = wrapper.Count;
    var dataCount = data.Count;
    return $"WrapperCount = {wrapperCount}, DataCount = {dataCount}";
}