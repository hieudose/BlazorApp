var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.BlazorApp_ApiService>("apiservice");

builder.AddProject<Projects.BlazorApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
