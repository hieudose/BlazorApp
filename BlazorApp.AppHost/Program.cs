using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache", 6379);

var apiService = builder.AddProject<Projects.BlazorApp_ApiService>("apiservice").WithReference(cache);


builder.AddProject<Projects.BlazorApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
