var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin(pgAdmin => pgAdmin.WithHostPort(5050))
    .WithLifetime(ContainerLifetime.Persistent);

var database = postgres.AddDatabase("stockinsight");

builder.AddProject<Projects.StockInsight_API>("api")
    .WithReference(database)
    .WaitFor(database)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health");

builder.Build().Run();
