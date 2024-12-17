using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using JsonGrpcService.Interface;
using JsonGrpcService.Models;
using JsonGrpcService.Services;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc().AddJsonTranscoding();

builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
});

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

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});


// Configure the HTTP request pipeline.
app.MapGrpcService<EmployeeDetailService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();