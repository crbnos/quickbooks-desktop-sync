using Microsoft.Extensions.Configuration;
using Supabase;
using Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace CarbonQuickBooks.Services
{
    public class CarbonService
    {
        private readonly IConfiguration _configuration;
        private Client? _client;

        private CarbonService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static async Task<CarbonService> CreateAsync(IConfiguration configuration)
        {
            var service = new CarbonService(configuration);
            await service.InitializeClientAsync();
            return service;
        }

        private async Task<Client?> CreateClient()
        {
            var url = _configuration["Supabase:Url"];
            var key = _configuration["Supabase:Key"];
            var apiKey = _configuration["Supabase:ApiKey"];

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(apiKey))
            {
                return null;
            }

            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                Headers = new Dictionary<string, string>
                {
                    { "carbon-key", apiKey }
                }
            };

            var client = new Client(url, key, options);
            await client.InitializeAsync();
            return client;
        }

        public async Task UpdateConfiguration()
        {
            await InitializeClientAsync();
        }

        private async Task InitializeClientAsync()
        {
            _client = await CreateClient();
        }

        public async Task<List<Customer>> GetCustomers()
        {
            if (_client == null)
            {
                return new List<Customer>();
            }

            try
            {
                var response = await _client.From<Customer>().Get();
                return response.Models;
            }
            catch (Exception ex)
            {
                // Log the exception details
                System.Diagnostics.Debug.WriteLine($"Error getting customers: {ex}");
                return new List<Customer>();
            }
        }

        public async Task<List<Supplier>> GetSuppliers()
        {
            if (_client == null)
            {
                return new List<Supplier>();
            }

            try
            {
                var response = await _client.From<Supplier>().Get();
                return response.Models;
            }
            catch (Exception ex)
            {
                // Log the exception details
                System.Diagnostics.Debug.WriteLine($"Error getting suppliers: {ex}");
                return new List<Supplier>();
            }
        }
    }
}