using CarbonQuickBooks.Models;
using CarbonQuickBooks.Services;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using IConfig = Microsoft.Extensions.Configuration.IConfiguration;
using ConfigManager = System.Configuration.ConfigurationManager;

namespace CarbonQuickBooks
{
    public partial class Form1 : Form
    {
        private readonly Configuration _config = ConfigManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private QuickBooksService? _qbService;
        private CarbonService? _carbonService;

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

            // Initial validation of settings
            ValidateSettings();
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
                    { "Supabase:Url", txtApiUrl.Text },
                    { "Supabase:Key", txtPublicKey.Text },
                    { "Supabase:ApiKey", txtApiKey.Text }
                };

                var configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(carbonConfig)
                    .Build();

                _carbonService = await CarbonService.CreateAsync(configuration);
                WriteToContactsDebugConsole("Carbon service initialized successfully.");
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
                txtCompanyFile.Text = _config.AppSettings.Settings["CompanyFile"]?.Value ?? string.Empty;
                txtApiUrl.Text = _config.AppSettings.Settings["ApiUrl"]?.Value ?? string.Empty;
                txtPublicKey.Text = _config.AppSettings.Settings["PublicKey"]?.Value ?? string.Empty;
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
                
                // Get and display Carbon customers
                WriteToContactsDebugConsole("\n=== Carbon Data ===");
                if (_carbonService != null)
                {
                    var carbonCustomers = await _carbonService.GetCustomers();
                    WriteToContactsDebugConsole($"\n--- Carbon Customers Found: {carbonCustomers.Count} ---");
                    foreach (var customer in carbonCustomers)
                    {
                        WriteToContactsDebugConsole($"Customer: {customer.Name}");
                        WriteToContactsDebugConsole("  ---");
                    }

                    var carbonSuppliers = await _carbonService.GetSuppliers();
                    WriteToContactsDebugConsole($"\n--- Carbon Suppliers Found: {carbonSuppliers.Count} ---");
                    foreach (var supplier in carbonSuppliers)
                    {
                        WriteToContactsDebugConsole($"Supplier: {supplier.Name}");
                        WriteToContactsDebugConsole("  ---");
                    }
                }

                WriteToContactsDebugConsole("=== Sync Complete ===\n");
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

        private async void BtnSyncInvoices_Click(object? sender, EventArgs e)
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
                    
                    // Get PO lines
                    var poLines = await _carbonService.GetPurchaseOrderLines(po.Id);
                    WriteToInvoiceDebugConsole($"Found {poLines.Count} lines for PO {po.Id}");

                    try {
                        // Add PO invoice to QuickBooks
                        _qbService.AddPurchaseOrderInvoice(po, poLines);
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
                    WriteToInvoiceDebugConsole($"  Shipment ID: {shipment.Id}");
                }

                WriteToInvoiceDebugConsole("\nSummary:");
                WriteToInvoiceDebugConsole($"- Total Purchase Orders Processed: {uninvoicedPOs.Count}");
                WriteToInvoiceDebugConsole($"- Total Shipments to Process: {uninvoicedShipments.Count}");
                
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
    }
}
