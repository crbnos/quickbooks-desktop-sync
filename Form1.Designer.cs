namespace CarbonQuickBooks
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabSettings = new TabPage();
            btnTestConnection = new Button();
            btnBrowseCompanyFile = new Button();
            btnSaveSettings = new Button();
            txtApiKey = new TextBox();
            txtPublicKey = new TextBox();
            txtApiUrl = new TextBox();
            txtCompanyFile = new TextBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            tabContacts = new TabPage();
            btnSyncVendors = new Button();
            btnSyncCustomers = new Button();
            listViewContacts = new ListView();
            tabInvoices = new TabPage();
            btnSyncSalesInvoices = new Button();
            btnSyncPurchaseInvoices = new Button();
            listViewInvoices = new ListView();
            tabControl1.SuspendLayout();
            tabSettings.SuspendLayout();
            tabContacts.SuspendLayout();
            tabInvoices.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabSettings);
            tabControl1.Controls.Add(tabContacts);
            tabControl1.Controls.Add(tabInvoices);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 450);
            tabControl1.TabIndex = 0;
            // 
            // tabSettings
            // 
            tabSettings.Controls.Add(btnTestConnection);
            tabSettings.Controls.Add(btnBrowseCompanyFile);
            tabSettings.Controls.Add(btnSaveSettings);
            tabSettings.Controls.Add(txtApiKey);
            tabSettings.Controls.Add(txtPublicKey);
            tabSettings.Controls.Add(txtApiUrl);
            tabSettings.Controls.Add(txtCompanyFile);
            tabSettings.Controls.Add(label4);
            tabSettings.Controls.Add(label3);
            tabSettings.Controls.Add(label2);
            tabSettings.Controls.Add(label1);
            tabSettings.Location = new Point(4, 24);
            tabSettings.Name = "tabSettings";
            tabSettings.Padding = new Padding(3);
            tabSettings.Size = new Size(792, 422);
            tabSettings.TabIndex = 0;
            tabSettings.Text = "Settings";
            tabSettings.UseVisualStyleBackColor = true;
            // 
            // btnTestConnection
            // 
            btnTestConnection.Location = new Point(120, 250);
            btnTestConnection.Name = "btnTestConnection";
            btnTestConnection.Size = new Size(120, 29);
            btnTestConnection.TabIndex = 10;
            btnTestConnection.Text = "Test Connection";
            btnTestConnection.UseVisualStyleBackColor = true;
            // 
            // btnBrowseCompanyFile
            // 
            btnBrowseCompanyFile.Location = new Point(430, 50);
            btnBrowseCompanyFile.Name = "btnBrowseCompanyFile";
            btnBrowseCompanyFile.Size = new Size(30, 23);
            btnBrowseCompanyFile.TabIndex = 9;
            btnBrowseCompanyFile.Text = "...";
            btnBrowseCompanyFile.UseVisualStyleBackColor = true;
            // 
            // btnSaveSettings
            // 
            btnSaveSettings.Location = new Point(20, 250);
            btnSaveSettings.Name = "btnSaveSettings";
            btnSaveSettings.Size = new Size(94, 29);
            btnSaveSettings.TabIndex = 8;
            btnSaveSettings.Text = "Save";
            btnSaveSettings.UseVisualStyleBackColor = true;
            // 
            // txtApiKey
            // 
            txtApiKey.Location = new Point(20, 200);
            txtApiKey.Name = "txtApiKey";
            txtApiKey.Size = new Size(400, 23);
            txtApiKey.TabIndex = 7;
            // 
            // txtPublicKey
            // 
            txtPublicKey.Location = new Point(20, 150);
            txtPublicKey.Name = "txtPublicKey";
            txtPublicKey.Size = new Size(400, 23);
            txtPublicKey.TabIndex = 6;
            // 
            // txtApiUrl
            // 
            txtApiUrl.Location = new Point(20, 100);
            txtApiUrl.Name = "txtApiUrl";
            txtApiUrl.Size = new Size(400, 23);
            txtApiUrl.TabIndex = 5;
            // 
            // txtCompanyFile
            // 
            txtCompanyFile.Location = new Point(20, 50);
            txtCompanyFile.Name = "txtCompanyFile";
            txtCompanyFile.Size = new Size(400, 23);
            txtCompanyFile.TabIndex = 4;
            txtCompanyFile.ReadOnly = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(20, 180);
            label4.Name = "label4";
            label4.Size = new Size(51, 15);
            label4.TabIndex = 3;
            label4.Text = "API Key:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 130);
            label3.Name = "label3";
            label3.Size = new Size(67, 15);
            label3.TabIndex = 2;
            label3.Text = "Public Key:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 80);
            label2.Name = "label2";
            label2.Size = new Size(54, 15);
            label2.TabIndex = 1;
            label2.Text = "API URL:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 30);
            label1.Name = "label1";
            label1.Size = new Size(86, 15);
            label1.TabIndex = 0;
            label1.Text = "Company File:";
            // 
            // tabContacts
            // 
            tabContacts.Controls.Add(btnSyncVendors);
            tabContacts.Controls.Add(btnSyncCustomers);
            tabContacts.Controls.Add(listViewContacts);
            tabContacts.Location = new Point(4, 24);
            tabContacts.Name = "tabContacts";
            tabContacts.Padding = new Padding(3);
            tabContacts.Size = new Size(792, 422);
            tabContacts.TabIndex = 1;
            tabContacts.Text = "Contacts";
            tabContacts.UseVisualStyleBackColor = true;
            // 
            // btnSyncVendors
            // 
            btnSyncVendors.Location = new Point(120, 20);
            btnSyncVendors.Name = "btnSyncVendors";
            btnSyncVendors.Size = new Size(94, 29);
            btnSyncVendors.TabIndex = 2;
            btnSyncVendors.Text = "Sync Vendors";
            btnSyncVendors.UseVisualStyleBackColor = true;
            // 
            // btnSyncCustomers
            // 
            btnSyncCustomers.Location = new Point(20, 20);
            btnSyncCustomers.Name = "btnSyncCustomers";
            btnSyncCustomers.Size = new Size(94, 29);
            btnSyncCustomers.TabIndex = 1;
            btnSyncCustomers.Text = "Sync Customers";
            btnSyncCustomers.UseVisualStyleBackColor = true;
            // 
            // listViewContacts
            // 
            listViewContacts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listViewContacts.Location = new Point(20, 60);
            listViewContacts.Name = "listViewContacts";
            listViewContacts.Size = new Size(752, 342);
            listViewContacts.TabIndex = 0;
            listViewContacts.UseCompatibleStateImageBehavior = false;
            listViewContacts.View = View.Details;
            // 
            // tabInvoices
            // 
            tabInvoices.Controls.Add(btnSyncSalesInvoices);
            tabInvoices.Controls.Add(btnSyncPurchaseInvoices);
            tabInvoices.Controls.Add(listViewInvoices);
            tabInvoices.Location = new Point(4, 24);
            tabInvoices.Name = "tabInvoices";
            tabInvoices.Size = new Size(792, 422);
            tabInvoices.TabIndex = 2;
            tabInvoices.Text = "Invoices";
            tabInvoices.UseVisualStyleBackColor = true;
            // 
            // btnSyncSalesInvoices
            // 
            btnSyncSalesInvoices.Location = new Point(120, 20);
            btnSyncSalesInvoices.Name = "btnSyncSalesInvoices";
            btnSyncSalesInvoices.Size = new Size(120, 29);
            btnSyncSalesInvoices.TabIndex = 2;
            btnSyncSalesInvoices.Text = "Sync Sales Invoices";
            btnSyncSalesInvoices.UseVisualStyleBackColor = true;
            // 
            // btnSyncPurchaseInvoices
            // 
            btnSyncPurchaseInvoices.Location = new Point(20, 20);
            btnSyncPurchaseInvoices.Name = "btnSyncPurchaseInvoices";
            btnSyncPurchaseInvoices.Size = new Size(94, 29);
            btnSyncPurchaseInvoices.TabIndex = 1;
            btnSyncPurchaseInvoices.Text = "Sync Purchase Invoices";
            btnSyncPurchaseInvoices.UseVisualStyleBackColor = true;
            // 
            // listViewInvoices
            // 
            listViewInvoices.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listViewInvoices.Location = new Point(20, 60);
            listViewInvoices.Name = "listViewInvoices";
            listViewInvoices.Size = new Size(752, 342);
            listViewInvoices.TabIndex = 0;
            listViewInvoices.UseCompatibleStateImageBehavior = false;
            listViewInvoices.View = View.Details;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "QuickBooks Sync";
            tabControl1.ResumeLayout(false);
            tabSettings.ResumeLayout(false);
            tabSettings.PerformLayout();
            tabContacts.ResumeLayout(false);
            tabInvoices.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabSettings;
        private TabPage tabContacts;
        private TabPage tabInvoices;
        private TextBox txtApiKey;
        private TextBox txtPublicKey;
        private TextBox txtApiUrl;
        private TextBox txtCompanyFile;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button btnSaveSettings;
        private Button btnBrowseCompanyFile;
        private Button btnTestConnection;
        private ListView listViewContacts;
        private Button btnSyncVendors;
        private Button btnSyncCustomers;
        private ListView listViewInvoices;
        private Button btnSyncSalesInvoices;
        private Button btnSyncPurchaseInvoices;
    }
}
