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
            tabInvoices = new TabPage();
            btnSyncInvoices = new Button();
            txtInvoicesDebugConsole = new TextBox();
            lblInvoicesDebugConsole = new Label();
            cboSyncInterval = new ComboBox();
            tabContacts = new TabPage();
            btnSyncContacts = new Button();
            txtDebugConsole = new TextBox();
            lblDebugConsole = new Label();
            tabSettings = new TabPage();
            btnTestConnection = new Button();
            btnBrowseCompanyFile = new Button();
            btnSaveSettings = new Button();
            txtApiKey = new TextBox();
            txtPublicKey = new TextBox();
            txtApiUrl = new TextBox();
            txtCompanyFile = new TextBox();
            txtPurchaseAccount = new TextBox();
            txtSalesRevenueAccount = new TextBox();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            lblVersion = new Label();
            tabControl1.SuspendLayout();
            tabInvoices.SuspendLayout();
            tabContacts.SuspendLayout();
            tabSettings.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabInvoices);
            tabControl1.Controls.Add(tabContacts);
            tabControl1.Controls.Add(tabSettings);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(4, 5, 4, 5);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1143, 750);
            tabControl1.TabIndex = 0;
            // 
            // tabInvoices
            // 
            tabInvoices.Controls.Add(btnSyncInvoices);
            tabInvoices.Controls.Add(txtInvoicesDebugConsole);
            tabInvoices.Controls.Add(lblInvoicesDebugConsole);
            tabInvoices.Controls.Add(cboSyncInterval);
            tabInvoices.Location = new Point(4, 34);
            tabInvoices.Margin = new Padding(4, 5, 4, 5);
            tabInvoices.Name = "tabInvoices";
            tabInvoices.Padding = new Padding(4, 5, 4, 5);
            tabInvoices.Size = new Size(1135, 712);
            tabInvoices.TabIndex = 2;
            tabInvoices.Text = "Invoices";
            tabInvoices.UseVisualStyleBackColor = true;
            // 
            // btnSyncInvoices
            // 
            btnSyncInvoices.Location = new Point(29, 34);
            btnSyncInvoices.Margin = new Padding(4, 5, 4, 5);
            btnSyncInvoices.Name = "btnSyncInvoices";
            btnSyncInvoices.Size = new Size(171, 48);
            btnSyncInvoices.TabIndex = 1;
            btnSyncInvoices.Text = "Sync Invoices";
            btnSyncInvoices.UseVisualStyleBackColor = true;
            // 
            // txtInvoicesDebugConsole
            // 
            txtInvoicesDebugConsole.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtInvoicesDebugConsole.BackColor = Color.Black;
            txtInvoicesDebugConsole.Font = new Font("Consolas", 9F);
            txtInvoicesDebugConsole.ForeColor = Color.LawnGreen;
            txtInvoicesDebugConsole.Location = new Point(29, 134);
            txtInvoicesDebugConsole.Margin = new Padding(4, 5, 4, 5);
            txtInvoicesDebugConsole.Multiline = true;
            txtInvoicesDebugConsole.Name = "txtInvoicesDebugConsole";
            txtInvoicesDebugConsole.ReadOnly = true;
            txtInvoicesDebugConsole.ScrollBars = ScrollBars.Vertical;
            txtInvoicesDebugConsole.Size = new Size(1070, 531);
            txtInvoicesDebugConsole.TabIndex = 3;
            // 
            // lblInvoicesDebugConsole
            // 
            lblInvoicesDebugConsole.AutoSize = true;
            lblInvoicesDebugConsole.Location = new Point(29, 100);
            lblInvoicesDebugConsole.Margin = new Padding(4, 0, 4, 0);
            lblInvoicesDebugConsole.Name = "lblInvoicesDebugConsole";
            lblInvoicesDebugConsole.Size = new Size(139, 25);
            lblInvoicesDebugConsole.TabIndex = 2;
            lblInvoicesDebugConsole.Text = "Debug Console:";
            // 
            // cboSyncInterval
            // 
            cboSyncInterval.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSyncInterval.FormattingEnabled = true;
            cboSyncInterval.IntegralHeight = false;
            cboSyncInterval.ItemHeight = 25;
            cboSyncInterval.Location = new Point(206, 43);
            cboSyncInterval.Margin = new Padding(2, 2, 2, 2);
            cboSyncInterval.Name = "cboSyncInterval";
            cboSyncInterval.Size = new Size(173, 33);
            cboSyncInterval.TabIndex = 4;
            // 
            // tabContacts
            // 
            tabContacts.Controls.Add(btnSyncContacts);
            tabContacts.Controls.Add(txtDebugConsole);
            tabContacts.Controls.Add(lblDebugConsole);
            tabContacts.Location = new Point(4, 34);
            tabContacts.Margin = new Padding(4, 5, 4, 5);
            tabContacts.Name = "tabContacts";
            tabContacts.Padding = new Padding(4, 5, 4, 5);
            tabContacts.Size = new Size(1135, 712);
            tabContacts.TabIndex = 1;
            tabContacts.Text = "Contacts";
            tabContacts.UseVisualStyleBackColor = true;
            // 
            // btnSyncContacts
            // 
            btnSyncContacts.Location = new Point(29, 34);
            btnSyncContacts.Margin = new Padding(4, 5, 4, 5);
            btnSyncContacts.Name = "btnSyncContacts";
            btnSyncContacts.Size = new Size(424, 48);
            btnSyncContacts.TabIndex = 1;
            btnSyncContacts.Text = "Sync Contacts from QuickBooks to Carbon";
            btnSyncContacts.UseVisualStyleBackColor = true;
            // 
            // txtDebugConsole
            // 
            txtDebugConsole.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtDebugConsole.BackColor = Color.Black;
            txtDebugConsole.Font = new Font("Consolas", 9F);
            txtDebugConsole.ForeColor = Color.LawnGreen;
            txtDebugConsole.Location = new Point(29, 134);
            txtDebugConsole.Margin = new Padding(4, 5, 4, 5);
            txtDebugConsole.Multiline = true;
            txtDebugConsole.Name = "txtDebugConsole";
            txtDebugConsole.ReadOnly = true;
            txtDebugConsole.ScrollBars = ScrollBars.Vertical;
            txtDebugConsole.Size = new Size(1070, 531);
            txtDebugConsole.TabIndex = 3;
            // 
            // lblDebugConsole
            // 
            lblDebugConsole.AutoSize = true;
            lblDebugConsole.Location = new Point(29, 100);
            lblDebugConsole.Margin = new Padding(4, 0, 4, 0);
            lblDebugConsole.Name = "lblDebugConsole";
            lblDebugConsole.Size = new Size(139, 25);
            lblDebugConsole.TabIndex = 2;
            lblDebugConsole.Text = "Debug Console:";
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
            tabSettings.Controls.Add(txtPurchaseAccount);
            tabSettings.Controls.Add(txtSalesRevenueAccount);
            tabSettings.Controls.Add(label6);
            tabSettings.Controls.Add(label5);
            tabSettings.Controls.Add(label4);
            tabSettings.Controls.Add(label3);
            tabSettings.Controls.Add(label2);
            tabSettings.Controls.Add(label1);
            tabSettings.Location = new Point(4, 34);
            tabSettings.Margin = new Padding(4, 5, 4, 5);
            tabSettings.Name = "tabSettings";
            tabSettings.Padding = new Padding(4, 5, 4, 5);
            tabSettings.Size = new Size(1135, 712);
            tabSettings.TabIndex = 0;
            tabSettings.Text = "Settings";
            tabSettings.UseVisualStyleBackColor = true;
            // 
            // btnTestConnection
            // 
            btnTestConnection.Location = new Point(171, 550);
            btnTestConnection.Margin = new Padding(4, 5, 4, 5);
            btnTestConnection.Name = "btnTestConnection";
            btnTestConnection.Size = new Size(171, 48);
            btnTestConnection.TabIndex = 10;
            btnTestConnection.Text = "Test Connection";
            btnTestConnection.UseVisualStyleBackColor = true;
            // 
            // btnBrowseCompanyFile
            // 
            btnBrowseCompanyFile.Location = new Point(614, 84);
            btnBrowseCompanyFile.Margin = new Padding(4, 5, 4, 5);
            btnBrowseCompanyFile.Name = "btnBrowseCompanyFile";
            btnBrowseCompanyFile.Size = new Size(43, 38);
            btnBrowseCompanyFile.TabIndex = 9;
            btnBrowseCompanyFile.Text = "...";
            btnBrowseCompanyFile.UseVisualStyleBackColor = true;
            // 
            // btnSaveSettings
            // 
            btnSaveSettings.Location = new Point(29, 550);
            btnSaveSettings.Margin = new Padding(4, 5, 4, 5);
            btnSaveSettings.Name = "btnSaveSettings";
            btnSaveSettings.Size = new Size(134, 48);
            btnSaveSettings.TabIndex = 8;
            btnSaveSettings.Text = "Save";
            btnSaveSettings.UseVisualStyleBackColor = true;
            // 
            // txtApiKey
            // 
            txtApiKey.Location = new Point(29, 334);
            txtApiKey.Margin = new Padding(4, 5, 4, 5);
            txtApiKey.Name = "txtApiKey";
            txtApiKey.Size = new Size(570, 31);
            txtApiKey.TabIndex = 7;
            txtApiKey.UseSystemPasswordChar = true;
            // 
            // txtPublicKey
            // 
            txtPublicKey.Location = new Point(29, 250);
            txtPublicKey.Margin = new Padding(4, 5, 4, 5);
            txtPublicKey.Name = "txtPublicKey";
            txtPublicKey.Size = new Size(570, 31);
            txtPublicKey.TabIndex = 6;
            txtPublicKey.UseSystemPasswordChar = true;
            // 
            // txtApiUrl
            // 
            txtApiUrl.Location = new Point(29, 166);
            txtApiUrl.Margin = new Padding(4, 5, 4, 5);
            txtApiUrl.Name = "txtApiUrl";
            txtApiUrl.Size = new Size(570, 31);
            txtApiUrl.TabIndex = 5;
            // 
            // txtCompanyFile
            // 
            txtCompanyFile.Location = new Point(29, 84);
            txtCompanyFile.Margin = new Padding(4, 5, 4, 5);
            txtCompanyFile.Name = "txtCompanyFile";
            txtCompanyFile.ReadOnly = true;
            txtCompanyFile.Size = new Size(570, 31);
            txtCompanyFile.TabIndex = 4;
            // 
            // txtPurchaseAccount
            // 
            txtPurchaseAccount.Location = new Point(29, 416);
            txtPurchaseAccount.Margin = new Padding(4, 5, 4, 5);
            txtPurchaseAccount.Name = "txtPurchaseAccount";
            txtPurchaseAccount.Size = new Size(570, 31);
            txtPurchaseAccount.TabIndex = 11;
            // 
            // txtSalesRevenueAccount
            // 
            txtSalesRevenueAccount.Location = new Point(29, 500);
            txtSalesRevenueAccount.Margin = new Padding(4, 5, 4, 5);
            txtSalesRevenueAccount.Name = "txtSalesRevenueAccount";
            txtSalesRevenueAccount.Size = new Size(570, 31);
            txtSalesRevenueAccount.TabIndex = 13;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(29, 466);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(197, 25);
            label6.TabIndex = 14;
            label6.Text = "Sales Revenue Account:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(29, 384);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(225, 25);
            label5.TabIndex = 12;
            label5.Text = "Purchase Expense Account:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(29, 300);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(76, 25);
            label4.TabIndex = 3;
            label4.Text = "API Key:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(29, 216);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(96, 25);
            label3.TabIndex = 2;
            label3.Text = "Public Key:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(29, 134);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(79, 25);
            label2.TabIndex = 1;
            label2.Text = "API URL:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(29, 50);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(124, 25);
            label1.TabIndex = 0;
            label1.Text = "Company File:";
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(ClientSize.Width - 100, ClientSize.Height - 40);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(100, 25);
            lblVersion.Text = "v0.1.0";
            lblVersion.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblVersion.BackColor = Color.Transparent;
            lblVersion.BringToFront();
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1143, 750);
            
            // Add version label first so it's on top
            lblVersion = new Label();
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(ClientSize.Width - 100, ClientSize.Height - 40);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(100, 25);
            lblVersion.Text = "v0.1.0";
            lblVersion.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblVersion.BackColor = Color.Transparent;
            lblVersion.BringToFront();
            Controls.Add(lblVersion);
            
            Controls.Add(tabControl1);
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "QuickBooks Sync";
            tabControl1.ResumeLayout(false);
            tabInvoices.ResumeLayout(false);
            tabInvoices.PerformLayout();
            tabContacts.ResumeLayout(false);
            tabContacts.PerformLayout();
            tabSettings.ResumeLayout(false);
            tabSettings.PerformLayout();
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
        private TextBox txtPurchaseAccount;
        private TextBox txtSalesRevenueAccount;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button btnSaveSettings;
        private Button btnBrowseCompanyFile;
        private Button btnTestConnection;
        private Button btnSyncContacts;
        private TextBox txtDebugConsole;
        private Label lblDebugConsole;
        private Button btnSyncInvoices;
        private TextBox txtInvoicesDebugConsole;
        private Label lblInvoicesDebugConsole;
        private ComboBox cboSyncInterval;
        private Label lblVersion;
    }
}
