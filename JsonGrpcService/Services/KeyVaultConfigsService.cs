namespace JsonGrpcService.Services
{
    public static class KeyVaultConfigsService
    {
        public class KeyVaultConfigs
        {
            public string? keyVaultUri { get; set; }
            public string? tenantId { get; set; }
            public string? clientId { get; set; }
            public string? clientSecret { get; set; }
        }
        public static KeyVaultConfigs GetConfigs(ConfigurationManager configuration)
        {
            KeyVaultConfigs configs = new KeyVaultConfigs()
            {
                keyVaultUri = configuration.GetValue<string>("KeyVaultConfig:KeyVaultUri"),
                tenantId = configuration.GetValue<string>("KeyVaultConfig:TenantId"),
                clientId = configuration.GetValue<string>("KeyVaultConfig:ClientId"),
                clientSecret = configuration.GetValue<string>("KeyVaultConfig:ClientSecretId")
            };
            return configs;
        }
    }
}
