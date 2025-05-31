using Microsoft.Extensions.Configuration;
using Supabase;
using static Supabase.Postgrest.Constants.Operator;
using Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using CarbonQuickBooks.Models;

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

        public async Task<List<Shipment>> GetUninvoicedShipments()
        {
            if (_client == null)
            {
                return new List<Shipment>();
            }

            try
            {
                var response = await _client.From<Shipment>()
                    .Select("*")
                    .Where(x => x.Invoiced == false)
                    .Get();
                return response.Models;
            }
            catch (Exception ex)
            {
                // Log the exception details
                System.Diagnostics.Debug.WriteLine($"Error getting uninvoiced shipments: {ex}");
                return new List<Shipment>();
            }
        }

        public async Task InsertContactReferences(List<Contact> contacts)
        {
            if (_client == null) return;

            try
            {
                foreach (var contact in contacts)
                {
                    if (contact.Type.Equals("Customer", StringComparison.OrdinalIgnoreCase))
                    {
                        var customer = new Customer
                        {
                            Name = !string.IsNullOrEmpty(contact.CompanyName) ? contact.CompanyName : contact.Name,
                            CreatedAt = DateTime.UtcNow,
                            ExternalId = new Dictionary<string, object>
                            {
                                { "quickbooks", contact.Name }
                            }
                        };

                        await _client.From<Customer>().Insert(customer);
                    }
                    else if (contact.Type.Equals("Supplier", StringComparison.OrdinalIgnoreCase))
                    {
                        var supplier = new Supplier
                        {
                            Name = !string.IsNullOrEmpty(contact.CompanyName) ? contact.CompanyName : contact.Name,
                            CreatedAt = DateTime.UtcNow,
                            ExternalId = new Dictionary<string, object>
                            {
                                { "quickbooks", contact.Name }
                            }
                        };

                        await _client.From<Supplier>().Insert(supplier);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error inserting contact references: {ex}");
                throw;
            }
        }

        public async Task<List<PurchaseOrder>> GetUninvoicedPurchaseOrders()
        {
            if (_client == null)
            {
                return new List<PurchaseOrder>();
            }

            try
            {
                var response = await _client.From<PurchaseOrder>()
                    .Select("*")
                    .Filter("status", In, new List<object> { "To Invoice", "To Receive and Invoice" })
                    .Get();
                return response.Models;
            }
            catch (Exception ex)
            {
                // Log the exception details
                System.Diagnostics.Debug.WriteLine($"Error getting uninvoiced purchase orders: {ex}");
                return new List<PurchaseOrder>();
            }
        }

        public async Task MarkPurchaseOrderAsInvoiced(PurchaseOrder purchaseOrder)
        {
            if (_client == null) return;
            
            await _client.From<PurchaseOrder>()
                .Where(x => x.Id == purchaseOrder.Id)
                .Set(x => x.Status, purchaseOrder.Status == "To Receive and Invoice" ? "To Receive" : "Completed")
                .Update();
        }

        public async Task<List<PurchaseOrderLine>> GetPurchaseOrderLines(string purchaseOrderId)
        {
            if (_client == null)
            {
                return new List<PurchaseOrderLine>();
            }

            try
            {
                var response = await _client.From<PurchaseOrderLine>()
                    .Select("*")
                    .Where(x => x.PurchaseOrderId == purchaseOrderId)
                    .Get();
                return response.Models;
            }
            catch (Exception ex)
            {
                // Log the exception details
                System.Diagnostics.Debug.WriteLine($"Error getting purchase order lines: {ex}");
                return new List<PurchaseOrderLine>();
            }
        }

        public async Task<Customer?> GetCustomerById(string customerId)
        {
            if (_client == null)
            {
                return null;
            }

            try
            {
                var response = await _client.From<Customer>()
                    .Select("*")
                    .Where(x => x.Id == customerId)
                    .Get();
                return response.Models.FirstOrDefault();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting customer by ID: {ex}");
                return null;
            }
        }

        public async Task<Supplier?> GetSupplierById(string supplierId)
        {
            if (_client == null)
            {
                return null;
            }

            try
            {
                var response = await _client.From<Supplier>()
                    .Select("*")
                    .Where(x => x.Id == supplierId)
                    .Get();
                return response.Models.FirstOrDefault();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting supplier by ID: {ex}");
                return null;
            }
        }

        public async Task UpdateCustomerExternalId(string customerId, string quickbooksName)
        {
            if (_client == null) return;

            try
            {
                var customer = await GetCustomerById(customerId);
                if (customer == null) return;

                var externalIds = customer.ExternalId ?? new Dictionary<string, object>();
                externalIds["quickbooks"] = quickbooksName;

                await _client.From<Customer>()
                    .Where(x => x.Id == customerId)
                    .Set(x => x.ExternalId, externalIds)
                    .Update();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating customer external ID: {ex}");
                throw;
            }
        }

        public async Task UpdateSupplierExternalId(string supplierId, string quickbooksName)
        {
            if (_client == null) return;

            try
            {
                var supplier = await GetSupplierById(supplierId);
                if (supplier == null) return;

                var externalIds = supplier.ExternalId ?? new Dictionary<string, object>();
                externalIds["quickbooks"] = quickbooksName;

                await _client.From<Supplier>()
                    .Where(x => x.Id == supplierId)
                    .Set(x => x.ExternalId, externalIds)
                    .Update();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating supplier external ID: {ex}");
                throw;
            }
        }
    }
}