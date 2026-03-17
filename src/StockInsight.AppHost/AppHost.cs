var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()                         
    .WithLifetime(ContainerLifetime.Persistent)  
    .AddDatabase("stockinsight");

var api = builder.AddProject<Projects.StockInsight_API>("api")
    .WithReference(postgres)
    .WaitFor(postgres)
    .WithExternalHttpEndpoints();

builder.Build().Run();