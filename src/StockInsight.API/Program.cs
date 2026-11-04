using Hangfire;
using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;
using StockInsight.Features;
using StockInsight.Infrastructure;
using StockInsight.Infrastructure.Persistence;
using StockInsight.Infrastructure.Seeds;
using StockInsight.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddInfrastructure();

builder.Services.AddFeatures();
builder.Services.AddHangfireServer();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi(options =>
{
    options.CreateSchemaReferenceId = type =>
        type.Type is { IsNested: true, DeclaringType: { } slice }
            ? $"{slice.Name}{type.Type.Name}"
            : OpenApiOptions.CreateDefaultSchemaReferenceId(type);
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    await app.ApplyMigrationsAsync();
    await app.SeedHistoricalPricesAsync();
    app.UseHangfireDashboard("/hangfire");
}

app.Services.GetRequiredService<IRecurringJobManager>().ScheduleFeatureJobs();

app.MapDefaultEndpoints();
app.MapFeatures();

app.Run();
