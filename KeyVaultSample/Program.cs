using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace KeyVaultSample
{
    class Program
    {
        private static IConfigurationRoot config;

        static void Main(string[] args)
        {
            config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
              
            KeyVaultClient keyVaultClient = new KeyVaultClient(GetAccessToken);  
  
            var secret = keyVaultClient.GetSecretAsync(config["KeyVault:Url"], "ConnectionString", "650884c5bcd3425a9b74a27e1f55a603").GetAwaiter().GetResult();  
  
            Console.WriteLine($"The super secret connection string is: {secret.Value}");
            Console.ReadKey();
        }

        private static async Task<string> GetAccessToken(string authority, string resource, string scope)  
        {  
            var clientId = config["AzureActiveDirectory:ClientId"];  
            var clientSecret = config["AzureActiveDirectory:ClientSecret"];  
            ClientCredential clientCredential = new ClientCredential(clientId, clientSecret);  
  
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);  
            var result = await context.AcquireTokenAsync(resource, clientCredential);  
  
            return result.AccessToken;  
        }
    }
}
