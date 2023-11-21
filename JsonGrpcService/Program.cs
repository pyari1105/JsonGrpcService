using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using JsonGrpcService.Interface;
using JsonGrpcService.Models;
using JsonGrpcService.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc().AddJsonTranscoding();

builder.Services.AddSingleton<IEmployeeService, EmployeeService>();

var configs = KeyVaultConfigsService.GetConfigs(builder.Configuration);
if (!string.IsNullOrWhiteSpace(configs.keyVaultUri))
{
    var credential = new ClientSecretCredential(configs.tenantId, configs.clientId, configs.clientSecret);
    var client = new SecretClient(new Uri(configs.keyVaultUri), credential);
    builder.Configuration.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());

    builder.Services.Configure<DemoDatabaseSettings>(
    builder.Configuration.GetSection(nameof(DemoDatabaseSettings)));

    builder.Services.AddSingleton<IDemoDatabaseSettings>(provider =>
    provider.GetRequiredService<IOptions<DemoDatabaseSettings>>().Value);
    builder.Services.AddControllers();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<EmployeeDetailService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();