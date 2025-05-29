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
            InitializeListViews();
            
            // Wire up event handlers
            btnBrowseCompanyFile.Click += BtnBrowseCompanyFile_Click;
            btnTestConnection.Click += BtnTestConnection_Click;
            btnSaveSettings.Click += BtnSaveSettings_Click;
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

        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void InitializeListViews()
        {
            // Initialize Contacts ListView
            listViewContacts.Columns.Add("Name", 200);
            listViewContacts.Columns.Add("Type", 100);
            listViewContacts.Columns.Add("Email", 200);
            listViewContacts.Columns.Add("Phone", 150);
            listViewContacts.FullRowSelect = true;
            listViewContacts.GridLines = true;

            // Initialize Invoices ListView
            listViewInvoices.Columns.Add("Invoice Number", 120);
            listViewInvoices.Columns.Add("Date", 100);
            listViewInvoices.Columns.Add("Type", 100);
            listViewInvoices.Columns.Add("Contact Name", 200);
            listViewInvoices.Columns.Add("Amount", 120);
            listViewInvoices.Columns.Add("Status", 100);
            listViewInvoices.FullRowSelect = true;
            listViewInvoices.GridLines = true;
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

        private void BtnSyncCustomers_Click(object sender, EventArgs e)
        {
            try
            {
                InitializeQBService();
                listViewContacts.Items.Clear();
                Cursor = Cursors.WaitCursor;

                var customers = _qbService?.GetCustomers();
                if (customers != null)
                {
                    foreach (var customer in customers)
                    {
                        var item = new ListViewItem(new[]
                        {
                            customer.Name,
                            customer.Type,
                            customer.Email,
                            customer.Phone
                        });
                        listViewContacts.Items.Add(item);
                    }
                }

                MessageBox.Show("Customer sync completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error syncing customers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                _qbService?.Dispose();
            }
        }

        private void BtnSyncVendors_Click(object sender, EventArgs e)
        {
            try
            {
                InitializeQBService();
                listViewContacts.Items.Clear();
                Cursor = Cursors.WaitCursor;

                var vendors = _qbService?.GetVendors();
                if (vendors != null)
                {
                    foreach (var vendor in vendors)
                    {
                        var item = new ListViewItem(new[]
                        {
                            vendor.Name,
                            vendor.Type,
                            vendor.Email,
                            vendor.Phone
                        });
                        listViewContacts.Items.Add(item);
                    }
                }

                MessageBox.Show("Vendor sync completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error syncing vendors: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                _qbService?.Dispose();
            }
        }

        private void BtnSyncPurchaseInvoices_Click(object sender, EventArgs e)
        {
            SyncInvoices(false);
        }

        private void BtnSyncSalesInvoices_Click(object sender, EventArgs e)
        {
            SyncInvoices(true);
        }

        private void SyncInvoices(bool salesInvoices)
        {
            try
            {
                InitializeQBService();
                listViewInvoices.Items.Clear();
                Cursor = Cursors.WaitCursor;

                var invoices = _qbService?.GetInvoices(salesInvoices);
                if (invoices != null)
                {
                    foreach (var invoice in invoices)
                    {
                        var item = new ListViewItem(new[]
                        {
                            invoice.InvoiceNumber,
                            invoice.Date.ToShortDateString(),
                            invoice.Type,
                            invoice.ContactName,
                            invoice.Amount.ToString("C"),
                            invoice.Status
                        });
                        listViewInvoices.Items.Add(item);
                    }
                }

                MessageBox.Show($"{(salesInvoices ? "Sales" : "Purchase")} invoice sync completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error syncing invoices: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                _qbService?.Dispose();
            }
        }

        private void BtnBrowseCompanyFile_Click(object sender, EventArgs e)
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

        private void BtnTestConnection_Click(object sender, EventArgs e)
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
