using CarbonQuickBooks.Models;
using CarbonQuickBooks.Services;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using IConfig = Microsoft.Extensions.Configuration.IConfiguration;
using ConfigManager = System.Configuration.ConfigurationManager;
using Models;

namespace CarbonQuickBooks
{
    public partial class Form1 : Form
    {
        private readonly Configuration _config = ConfigManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private QuickBooksService? _qbService;
        private CarbonService? _carbonService;
        private System.Windows.Forms.Timer? _syncTimer;

        public async Task InitializeAsync()
        {
            LoadSettings();
            await InitializeCarbonServiceAsync().ConfigureAwait(false);
            WriteToContactsDebugConsole("Ready for logging...\n");
            WriteToInvoiceDebugConsole("Ready for logging...\n");
            
            // Wire up event handlers
            btnBrowseCompanyFile.Click += BtnBrowseCompanyFile_Click;
            btnTestConnection.Click += BtnTestConnection_Click;
            btnSaveSettings.Click += BtnSaveSettings_Click;
            btnSyncContacts.Click += BtnSyncContacts_Click;
            btnSyncInvoices.Click += BtnSyncInvoices_Click;

            // Add text changed handlers for Carbon settings
            txtApiUrl.TextChanged += CarbonSettings_TextChanged;
            txtPublicKey.TextChanged += CarbonSettings_TextChanged;
            txtApiKey.TextChanged += CarbonSettings_TextChanged;
            txtPurchaseAccount.TextChanged += CarbonSettings_TextChanged;
            txtSalesRevenueAccount.TextChanged += CarbonSettings_TextChanged;
            txtCompanyFile.TextChanged += CarbonSettings_TextChanged;

            // Setup sync interval dropdown
            cboSyncInterval.Items.AddRange(new string[] {
                "Manual",
                "Every 10 minutes",
                "Every 30 minutes", 
                "Every hour",
                "Every 4 hours"
            });
            cboSyncInterval.SelectedIndex = 0;
            cboSyncInterval.SelectedIndexChanged += CboSyncInterval_SelectedIndexChanged;

            // Initialize timer
            _syncTimer = new System.Windows.Forms.Timer();
            _syncTimer.Tick += async (s, e) => await SyncInvoices();

            // Initial validation of settings
            ValidateSettings();
        }

        private void CboSyncInterval_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_syncTimer == null) return;

            // Stop existing timer
            _syncTimer.Stop();

            // Set new interval based on selection
            switch (cboSyncInterval.SelectedIndex)
            {
                case 0: // Manual
                    break;
                case 1: // 10 minutes
                    _syncTimer.Interval = 10 * 60 * 1000;
                    _syncTimer.Start();
                    break;
                case 2: // 30 minutes
                    _syncTimer.Interval = 30 * 60 * 1000;
                    _syncTimer.Start();
                    break;
                case 3: // 1 hour
                    _syncTimer.Interval = 60 * 60 * 1000;
                    _syncTimer.Start();
                    break;
                case 4: // 4 hours
                    _syncTimer.Interval = 4 * 60 * 60 * 1000;
                    _syncTimer.Start();
                    break;
            }
        }

        private async Task SyncInvoices()
        {
            if (btnSyncInvoices.Enabled)
            {
                await BtnSyncInvoices_ClickAsync();
            }
        }

        public Form1()
        {
            InitializeComponent();
            _ = InitializeAsync(); // Fire and forget initialization
        }

        private void ValidateSettings()
        {
            bool isValid = !string.IsNullOrWhiteSpace(txtCompanyFile.Text) &&
                          !string.IsNullOrWhiteSpace(txtApiUrl.Text) &&
                          !string.IsNullOrWhiteSpace(txtPublicKey.Text) &&
                          !string.IsNullOrWhiteSpace(txtApiKey.Text) &&
                          !string.IsNullOrWhiteSpace(txtPurchaseAccount.Text) &&
                          !string.IsNullOrWhiteSpace(txtSalesRevenueAccount.Text);

            btnSyncInvoices.Enabled = isValid;
            btnSyncContacts.Enabled = isValid;  // btnSync is the contacts sync button
        }

        private async Task InitializeCarbonServiceAsync()
        {
            try
            {
                var carbonConfig = new Dictionary<string, string?>
                {
                    { "Supabase:Url", String.IsNullOrEmpty(txtApiUrl.Text) ? "https://sqojijiijknhbgyogmlu.supabase.co" : txtApiUrl.Text },
                    { "Supabase:Key", String.IsNullOrEmpty(txtPublicKey.Text) ? "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNxb2ppamlpamtuaGJneW9nbWx1Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MjM2MDU0MzksImV4cCI6MjAzOTE4MTQzOX0.JMzLs9Y4Y4kQ-jhQHrSqgNyHSZgrkwzBd1PwPbVPtbQ" : txtPublicKey.Text },
                    { "Supabase:ApiKey", txtApiKey.Text }
                };

                var configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(carbonConfig)
                    .Build();

                _carbonService = await CarbonService.CreateAsync(configuration);
                WriteToContactsDebugConsole("Carbon service initialized successfully.");
                WriteToInvoiceDebugConsole("Carbon service initialized successfully.");
            }
            catch (Exception ex)
            {
                WriteToContactsDebugConsole($"Error initializing Carbon service: {ex.Message}");
            }
        }

        private async void CarbonSettings_TextChanged(object? sender, EventArgs e)
        {
            ValidateSettings();
            if (_carbonService != null)
            {
                await InitializeCarbonServiceAsync().ConfigureAwait(false);
                WriteToContactsDebugConsole("Carbon configuration updated.");
            }
        }

        private void LoadSettings()
        {
            try
            {
                // Default values
                const string defaultApiUrl = "https://sqojijiijknhbgyogmlu.supabase.co";
                const string defaultPublicKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNxb2ppamlpamtuaGJneW9nbWx1Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MjM2MDU0MzksImV4cCI6MjAzOTE4MTQzOX0.JMzLs9Y4Y4kQ-jhQHrSqgNyHSZgrkwzBd1PwPbVPtbQ";

                txtCompanyFile.Text = _config.AppSettings.Settings["CompanyFile"]?.Value ?? string.Empty;
                
                // Set API URL with default if null or empty
                var apiUrl = _config.AppSettings.Settings["ApiUrl"]?.Value;
                txtApiUrl.Text = string.IsNullOrEmpty(apiUrl) ? defaultApiUrl : apiUrl;
                
                // Set Public Key with default if null or empty
                var publicKey = _config.AppSettings.Settings["PublicKey"]?.Value;
                txtPublicKey.Text = string.IsNullOrEmpty(publicKey) ? defaultPublicKey : publicKey;
                
                txtApiKey.Text = _config.AppSettings.Settings["ApiKey"]?.Value ?? string.Empty;
                txtPurchaseAccount.Text = _config.AppSettings.Settings["PurchaseAccount"]?.Value ?? string.Empty;
                txtSalesRevenueAccount.Text = _config.AppSettings.Settings["SalesRevenueAccount"]?.Value ?? string.Empty;
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SaveSettings()
        {
            try
            {
                // Update configuration settings
                UpdateOrAddSetting("CompanyFile", txtCompanyFile.Text);
                UpdateOrAddSetting("ApiUrl", txtApiUrl.Text);
                UpdateOrAddSetting("PublicKey", txtPublicKey.Text);
                UpdateOrAddSetting("ApiKey", txtApiKey.Text);
                UpdateOrAddSetting("PurchaseAccount", txtPurchaseAccount.Text);
                UpdateOrAddSetting("SalesRevenueAccount", txtSalesRevenueAccount.Text);

                // Save the configuration
                _config.Save(ConfigurationSaveMode.Modified);
                ConfigManager.RefreshSection("appSettings");

                // Update Carbon service with new settings
                await InitializeCarbonServiceAsync().ConfigureAwait(false);
                if (_carbonService != null)
                {
                    _carbonService.UpdateConfiguration();
                }

                MessageBox.Show("Settings saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateOrAddSetting(string key, string value)
        {
            if (_config.AppSettings.Settings[key] == null)
            {
                _config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                _config.AppSettings.Settings[key].Value = value;
            }
        }

        private async void BtnSaveSettings_Click(object? sender, EventArgs e)
        {
            await SaveSettings();
        }

        private async Task InitializeServicesAsync()
        {
            string companyFile = txtCompanyFile.Text;
            if (string.IsNullOrEmpty(companyFile))
            {
                throw new Exception("Company file path is not set. Please configure it in the Settings tab.");
            }
            _qbService = new QuickBooksService(companyFile);

            // Create a new configuration for Carbon service
            var carbonConfig = new Dictionary<string, string?>
            {
                { "Supabase:Url", txtApiUrl.Text },
                { "Supabase:Key", txtPublicKey.Text },
                { "Supabase:ApiKey", txtApiKey.Text }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(carbonConfig)
                .Build();

            _carbonService = await CarbonService.CreateAsync(configuration);
        }

        private void WriteToContactsDebugConsole(string message)
        {
            if (txtDebugConsole.InvokeRequired)
            {
                txtDebugConsole.Invoke(new Action(() => WriteToContactsDebugConsole(message)));
                return;
            }
            
            txtDebugConsole.AppendText(message + Environment.NewLine);
            txtDebugConsole.SelectionStart = txtDebugConsole.Text.Length;
            txtDebugConsole.ScrollToCaret();
        }

        private void WriteToInvoiceDebugConsole(string message)
        {
            if (txtInvoicesDebugConsole.InvokeRequired)
            {
                txtInvoicesDebugConsole.Invoke(new Action(() => WriteToInvoiceDebugConsole(message)));
                return;
            }
            
            txtInvoicesDebugConsole.AppendText(message + Environment.NewLine);
            txtInvoicesDebugConsole.SelectionStart = txtInvoicesDebugConsole.Text.Length;
            txtInvoicesDebugConsole.ScrollToCaret();
        }

        private async void BtnSyncContacts_Click(object? sender, EventArgs e)
        {
            try
            {
                await InitializeServicesAsync();
                Cursor = Cursors.WaitCursor;

                WriteToContactsDebugConsole("=== Starting QuickBooks Sync ===");
                
                // Get QuickBooks contacts
                WriteToContactsDebugConsole("\n=== QuickBooks Data ===");
                var quickbooksCustomers = _qbService?.GetCustomers() ?? new List<Contact>();
                WriteToContactsDebugConsole($"\n--- QuickBooks Customers Found: {quickbooksCustomers.Count} ---");
                foreach (var customer in quickbooksCustomers)
                {
                    WriteToContactsDebugConsole($"Customer: {customer.Name}");
                }

                var quickbooksVendors = _qbService?.GetVendors() ?? new List<Contact>();
                WriteToContactsDebugConsole($"\n--- QuickBooks Vendors Found: {quickbooksVendors.Count} ---");
                foreach (var vendor in quickbooksVendors)
                {
                    WriteToContactsDebugConsole($"Vendor: {vendor.Name}");
                }

                // Get Carbon contacts
                WriteToContactsDebugConsole("\n=== Carbon Data ===");
                if (_carbonService != null)
                {
                    var carbonCustomers = await _carbonService.GetCustomers();
                    WriteToContactsDebugConsole($"\n--- Carbon Customers Found: {carbonCustomers.Count} ---");
                    
                    // Find customers that need to be synced
                    var customersToSync = quickbooksCustomers.Where(qbCustomer => 
                        !carbonCustomers.Any(cc => 
                            cc.ExternalId != null && 
                            cc.ExternalId.ContainsKey("quickbooks") && 
                            cc.ExternalId["quickbooks"].ToString().ToLower() == qbCustomer.Name.ToLower()
                        )).ToList();

                    WriteToContactsDebugConsole($"\n--- Customers to Sync: {customersToSync.Count} ---");
                    foreach (var customer in customersToSync)
                    {
                        WriteToContactsDebugConsole($"Adding customer: {customer.Name}");
                        await _carbonService.InsertCustomer(customer);
                    }

                    var carbonSuppliers = await _carbonService.GetSuppliers();
                    WriteToContactsDebugConsole($"\n--- Carbon Suppliers Found: {carbonSuppliers.Count} ---");
                    
                    // Find vendors that need to be synced
                    var vendorsToSync = quickbooksVendors.Where(qbVendor => 
                        !carbonSuppliers.Any(cs => 
                            cs.ExternalId != null && 
                            cs.ExternalId.ContainsKey("quickbooks") && 
                            cs.ExternalId["quickbooks"].ToString().ToLower() == qbVendor.Name.ToLower()
                        )).ToList();

                    WriteToContactsDebugConsole($"\n--- Vendors to Sync: {vendorsToSync.Count} ---");
                    foreach (var vendor in vendorsToSync)
                    {
                        WriteToContactsDebugConsole($"Adding vendor: {vendor.Name}");
                        await _carbonService.InsertSupplier(vendor);
                    }

                    WriteToContactsDebugConsole($"\nSync Summary:");
                    WriteToContactsDebugConsole($"- QuickBooks Customers: {quickbooksCustomers.Count}");
                    WriteToContactsDebugConsole($"- QuickBooks Vendors: {quickbooksVendors.Count}");
                    WriteToContactsDebugConsole($"- Carbon Customers: {carbonCustomers.Count}");
                    WriteToContactsDebugConsole($"- Carbon Suppliers: {carbonSuppliers.Count}");
                    WriteToContactsDebugConsole($"- Customers Added: {customersToSync.Count}");
                    WriteToContactsDebugConsole($"- Vendors Added: {vendorsToSync.Count}");
                }

                WriteToContactsDebugConsole("\n=== Sync Complete ===\n");
            }
            catch (Exception ex)
            {
                WriteToContactsDebugConsole($"ERROR during sync: {ex.Message}");
                MessageBox.Show($"Error during sync: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                _qbService?.Dispose();
            }
        }

        private void BtnBrowseCompanyFile_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "QuickBooks Files (*.qbw)|*.qbw|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtCompanyFile.Text = openFileDialog.FileName;
                    SaveSettings(); // Save settings when company file is selected
                }
            }
        }

        private void BtnTestConnection_Click(object? sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                using (var qbService = new QuickBooksService(txtCompanyFile.Text))
                {
                    qbService.TestConnection();
                    MessageBox.Show("Successfully connected to QuickBooks!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect to QuickBooks: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async Task BtnSyncInvoices_ClickAsync()
        {
            try
            {
                await InitializeServicesAsync();
                Cursor = Cursors.WaitCursor;

                WriteToInvoiceDebugConsole("\n=== Starting Invoice Sync Process ===");
                WriteToInvoiceDebugConsole("Initializing services...");
                
                if (_carbonService == null)
                {
                    throw new Exception("Carbon service is not initialized");
                }

                if (_qbService == null) 
                {
                    throw new Exception("QuickBooks service is not initialized");
                }

                WriteToInvoiceDebugConsole("\nFetching uninvoiced purchase orders...");
                var uninvoicedPOs = await _carbonService.GetUninvoicedPurchaseOrders();
                WriteToInvoiceDebugConsole($"Found {uninvoicedPOs.Count} uninvoiced purchase orders");

                foreach (var po in uninvoicedPOs)
                {
                    WriteToInvoiceDebugConsole($"\nProcessing Purchase Order: {po.Id}");

                    // Get supplier details from Carbon
                    var supplier = await _carbonService.GetSupplierById(po.SupplierId);
                    if (supplier == null)
                    {
                        WriteToInvoiceDebugConsole($"Failed to find supplier {po.SupplierId} in Carbon");
                        continue;
                    }

                    // Check if supplier exists in QuickBooks
                    string quickbooksSupplierName = supplier.Name;
                    bool supplierExists = _qbService.VendorExists(quickbooksSupplierName);

                    if (!supplierExists)
                    {
                        WriteToInvoiceDebugConsole($"Creating supplier {supplier.Name} in QuickBooks...");
                        var supplierToAdd = new Supplier
                        {
                            Name = supplier.Name,
                            // Add other fields as needed
                        };
                        _qbService.AddSupplier(supplierToAdd);
                        
                        // Update Carbon with QuickBooks external ID
                        await _carbonService.UpdateSupplierExternalId(supplier.Id, quickbooksSupplierName);
                        WriteToInvoiceDebugConsole($"Supplier {supplier.Name} created in QuickBooks and updated in Carbon");
                    }
                    
                    // Get PO lines
                    var poLines = await _carbonService.GetPurchaseOrderLines(po.Id);
                    WriteToInvoiceDebugConsole($"Found {poLines.Count} lines for PO {po.Id}");

                    try {
                        // Update PO with QuickBooks supplier name
                        po.SupplierId = quickbooksSupplierName;
                        
                        // Add PO invoice to QuickBooks
                        _qbService.AddPurchaseOrderInvoice(po, poLines, txtPurchaseAccount.Text);
                        await _carbonService.MarkPurchaseOrderAsInvoiced(po);
                        WriteToInvoiceDebugConsole($"Successfully created QB invoice for PO {po.Id}");
                    }
                    catch (Exception ex) {
                        WriteToInvoiceDebugConsole($"Failed to create QB invoice for PO {po.Id}: {ex.Message}");
                        continue;
                    }
                }

                WriteToInvoiceDebugConsole("\nFetching uninvoiced shipments...");
                var uninvoicedShipments = await _carbonService.GetUninvoicedShipments();
                WriteToInvoiceDebugConsole($"Found {uninvoicedShipments.Count} uninvoiced shipments");

                foreach (var shipment in uninvoicedShipments)
                {
                    WriteToInvoiceDebugConsole($"\nProcessing Shipment: {shipment.Id}");

                    try {
                        // Get shipment lines
                        if(shipment.SourceDocument == "Sales Order") {
                            var shipmentLines = await _carbonService.GetShipmentLines(shipment.Id);
                            var salesOrder = await _carbonService.GetSalesOrderById(shipment.SourceDocumentId);
                            if (salesOrder == null)
                            {
                                WriteToInvoiceDebugConsole($"Failed to find sales order {shipment.SourceDocumentId}");
                                continue;
                            }

                            // Get customer details from Carbon
                            var customer = await _carbonService.GetCustomerById(salesOrder.CustomerId);
                            if (customer == null)
                            {
                                WriteToInvoiceDebugConsole($"Failed to find customer {salesOrder.CustomerId} in Carbon");
                                continue;
                            }

                            // Check if customer exists in QuickBooks
                            string quickbooksCustomerName = customer.Name;
                            bool customerExists = _qbService.CustomerExists(quickbooksCustomerName);

                            if (!customerExists)
                            {
                                WriteToInvoiceDebugConsole($"Creating customer {customer.Name} in QuickBooks...");
                                var customerToAdd = new Contact
                                {
                                    Name = customer.Name,
                                    Type = "Customer",
                                    CompanyName = customer.Name
                                    // Add other fields as needed
                                };
                                _qbService.AddContact(customerToAdd);
                                
                                // Update Carbon with QuickBooks external ID
                                await _carbonService.UpdateCustomerExternalId(customer.Id, quickbooksCustomerName);
                                WriteToInvoiceDebugConsole($"Customer {customer.Name} created in QuickBooks and updated in Carbon");
                            }

                            // Get sales order lines
                            var salesOrderLines = await _carbonService.GetSalesOrderLines(salesOrder.Id);

                            // Add shipment invoice to QuickBooks using the verified customer name
                            _qbService.AddShipmentInvoiceFromSalesOrder(shipment, shipmentLines, salesOrder, salesOrderLines, quickbooksCustomerName, txtSalesRevenueAccount.Text, txtPurchaseAccount.Text);
                            
                            // Mark shipment as invoiced in Carbon
                            await _carbonService.MarkShipmentAsInvoiced(shipment, shipmentLines, salesOrder, salesOrderLines);
                            WriteToInvoiceDebugConsole($"Successfully created QB invoice for shipment {shipment.Id}");
                        }
                    }
                    catch (Exception ex) {
                        WriteToInvoiceDebugConsole($"Failed to create QB invoice for shipment {shipment.Id}: {ex.Message}");
                        continue;
                    }
                }

                WriteToInvoiceDebugConsole("\nSummary:");
                WriteToInvoiceDebugConsole($"- Total Purchase Orders Processed: {uninvoicedPOs.Count}");
                WriteToInvoiceDebugConsole($"- Total Shipments Processed: {uninvoicedShipments.Count}");
                
                WriteToInvoiceDebugConsole("\n=== Invoice Sync Process Complete ===");
            }
            catch (Exception ex)
            {
                WriteToInvoiceDebugConsole($"\nERROR during invoice sync: {ex.Message}");
                WriteToInvoiceDebugConsole($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Error during invoice sync: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                _qbService?.Dispose();
            }
        }

        private async void BtnSyncInvoices_Click(object? sender, EventArgs e)
        {
            await BtnSyncInvoices_ClickAsync();
        }
    }
}