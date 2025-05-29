using CarbonQuickBooks.Models;
using CarbonQuickBooks.Services;
using System.Configuration;

namespace CarbonQuickBooks
{
    public partial class Form1 : Form
    {
        private readonly Configuration _config;
        private QuickBooksService? _qbService;

        public Form1()
        {
            InitializeComponent();
            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            LoadSettings();
            
            WriteToDebugConsole("QuickBooks Sync application started. Ready for logging...\n");
            
            // Wire up event handlers
            btnBrowseCompanyFile.Click += BtnBrowseCompanyFile_Click;
            btnTestConnection.Click += BtnTestConnection_Click;
            btnSaveSettings.Click += BtnSaveSettings_Click;
            btnSync.Click += BtnSync_Click;
        }

        private void LoadSettings()
        {
            try
            {
                txtCompanyFile.Text = _config.AppSettings.Settings["CompanyFile"]?.Value ?? string.Empty;
                txtApiUrl.Text = _config.AppSettings.Settings["ApiUrl"]?.Value ?? string.Empty;
                txtPublicKey.Text = _config.AppSettings.Settings["PublicKey"]?.Value ?? string.Empty;
                txtApiKey.Text = _config.AppSettings.Settings["ApiKey"]?.Value ?? string.Empty;
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveSettings()
        {
            try
            {
                // Update configuration settings
                UpdateOrAddSetting("CompanyFile", txtCompanyFile.Text);
                UpdateOrAddSetting("ApiUrl", txtApiUrl.Text);
                UpdateOrAddSetting("PublicKey", txtPublicKey.Text);
                UpdateOrAddSetting("ApiKey", txtApiKey.Text);

                // Save the configuration
                _config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

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

        private void BtnSaveSettings_Click(object? sender, EventArgs e)
        {
            SaveSettings();
        }

        private void InitializeQBService()
        {
            string companyFile = txtCompanyFile.Text;
            if (string.IsNullOrEmpty(companyFile))
            {
                throw new Exception("Company file path is not set. Please configure it in the Settings tab.");
            }
            _qbService = new QuickBooksService(companyFile);
        }

        private void WriteToDebugConsole(string message)
        {
            if (txtDebugConsole.InvokeRequired)
            {
                txtDebugConsole.Invoke(new Action(() => WriteToDebugConsole(message)));
                return;
            }
            
            txtDebugConsole.AppendText(message + Environment.NewLine);
            txtDebugConsole.SelectionStart = txtDebugConsole.Text.Length;
            txtDebugConsole.ScrollToCaret();
        }

        private void BtnSync_Click(object? sender, EventArgs e)
        {
            try
            {
                InitializeQBService();
                Cursor = Cursors.WaitCursor;

                WriteToDebugConsole("=== Starting QuickBooks Sync ===");
                
                // Sync both customers and vendors
                var customers = _qbService?.GetCustomers();
                var vendors = _qbService?.GetVendors();

                int customerCount = customers?.Count ?? 0;
                int vendorCount = vendors?.Count ?? 0;

                WriteToDebugConsole($"\n--- Customers Found: {customerCount} ---");
                if (customers != null)
                {
                    foreach (var customer in customers)
                    {
                        WriteToDebugConsole($"Customer: {customer.Name}");
                        WriteToDebugConsole($"  Company: {customer.CompanyName ?? "N/A"}");
                        WriteToDebugConsole($"  Type: {customer.Type}");
                        WriteToDebugConsole($"  Email: {customer.Email ?? "N/A"}");
                        WriteToDebugConsole($"  Phone: {customer.Phone ?? "N/A"}");
                        
                        // Display billing address
                        if (customer.BillingAddress != null)
                        {
                            WriteToDebugConsole($"  Billing Address:");
                            var billingLines = customer.BillingAddress.ToString().Split('\n');
                            foreach (var line in billingLines)
                            {
                                if (!string.IsNullOrWhiteSpace(line))
                                    WriteToDebugConsole($"    {line.Trim()}");
                            }
                        }
                        else
                        {
                            WriteToDebugConsole($"  Billing Address: N/A");
                        }
                        
                        // Display shipping address
                        if (customer.ShippingAddress != null)
                        {
                            WriteToDebugConsole($"  Shipping Address:");
                            var shippingLines = customer.ShippingAddress.ToString().Split('\n');
                            foreach (var line in shippingLines)
                            {
                                if (!string.IsNullOrWhiteSpace(line))
                                    WriteToDebugConsole($"    {line.Trim()}");
                            }
                        }
                        else
                        {
                            WriteToDebugConsole($"  Shipping Address: N/A");
                        }
                        
                        WriteToDebugConsole("  ---");
                    }
                }

                WriteToDebugConsole($"\n--- Vendors Found: {vendorCount} ---");
                if (vendors != null)
                {
                    foreach (var vendor in vendors)
                    {
                        WriteToDebugConsole($"Vendor: {vendor.Name}");
                        WriteToDebugConsole($"  Company: {vendor.CompanyName ?? "N/A"}");
                        WriteToDebugConsole($"  Type: {vendor.Type}");
                        WriteToDebugConsole($"  Email: {vendor.Email ?? "N/A"}");
                        WriteToDebugConsole($"  Phone: {vendor.Phone ?? "N/A"}");
                        
                        // Display billing address
                        if (vendor.BillingAddress != null)
                        {
                            WriteToDebugConsole($"  Billing Address:");
                            var billingLines = vendor.BillingAddress.ToString().Split('\n');
                            foreach (var line in billingLines)
                            {
                                if (!string.IsNullOrWhiteSpace(line))
                                    WriteToDebugConsole($"    {line.Trim()}");
                            }
                        }
                        else
                        {
                            WriteToDebugConsole($"  Billing Address: N/A");
                        }
                        
                        // Display shipping address
                        if (vendor.ShippingAddress != null)
                        {
                            WriteToDebugConsole($"  Shipping Address:");
                            var shippingLines = vendor.ShippingAddress.ToString().Split('\n');
                            foreach (var line in shippingLines)
                            {
                                if (!string.IsNullOrWhiteSpace(line))
                                    WriteToDebugConsole($"    {line.Trim()}");
                            }
                        }
                        else
                        {
                            WriteToDebugConsole($"  Shipping Address: N/A");
                        }
                        
                        WriteToDebugConsole("  ---");
                    }
                }
                WriteToDebugConsole("=== Sync Complete ===\n");

                MessageBox.Show($"Sync completed successfully!\nCustomers: {customerCount}\nVendors: {vendorCount}\n\nContact details logged to debug console.", 
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                WriteToDebugConsole($"ERROR during sync: {ex.Message}");
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
    }
}
