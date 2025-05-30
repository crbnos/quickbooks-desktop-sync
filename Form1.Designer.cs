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
            txtPurchaseAccount = new TextBox();
            txtSalesRevenueAccount = new TextBox();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            tabContacts = new TabPage();
            btnSyncContacts = new Button();
            txtDebugConsole = new TextBox();
            lblDebugConsole = new Label();
            tabInvoices = new TabPage();
            btnSyncInvoices = new Button();
            txtInvoicesDebugConsole = new TextBox();
            lblInvoicesDebugConsole = new Label();
            tabControl1.SuspendLayout();
            tabSettings.SuspendLayout();
            tabContacts.SuspendLayout();
            tabInvoices.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabInvoices);
            tabControl1.Controls.Add(tabContacts);
            tabControl1.Controls.Add(tabSettings);
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
            tabSettings.Controls.Add(txtPurchaseAccount);
            tabSettings.Controls.Add(txtSalesRevenueAccount);
            tabSettings.Controls.Add(label6);
            tabSettings.Controls.Add(label5);
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
            btnTestConnection.Location = new Point(120, 330);
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
            btnSaveSettings.Location = new Point(20, 330);
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
            txtApiKey.UseSystemPasswordChar = true;
            // 
            // txtPublicKey
            // 
            txtPublicKey.Location = new Point(20, 150);
            txtPublicKey.Name = "txtPublicKey";
            txtPublicKey.Size = new Size(400, 23);
            txtPublicKey.TabIndex = 6;
            txtPublicKey.UseSystemPasswordChar = true;
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
            // txtPurchaseAccount
            // 
            txtPurchaseAccount.Location = new Point(20, 250);
            txtPurchaseAccount.Name = "txtPurchaseAccount";
            txtPurchaseAccount.Size = new Size(400, 23);
            txtPurchaseAccount.TabIndex = 11;
            // 
            // txtSalesRevenueAccount
            // 
            txtSalesRevenueAccount.Location = new Point(20, 300);
            txtSalesRevenueAccount.Name = "txtSalesRevenueAccount";
            txtSalesRevenueAccount.Size = new Size(400, 23);
            txtSalesRevenueAccount.TabIndex = 13;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(20, 280);
            label6.Name = "label6";
            label6.Size = new Size(127, 15);
            label6.TabIndex = 14;
            label6.Text = "Sales Revenue Account:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(20, 230);
            label5.Name = "label5";
            label5.Size = new Size(102, 15);
            label5.TabIndex = 12;
            label5.Text = "Purchase Expense Account:";
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
            tabContacts.Controls.Add(btnSyncContacts);
            tabContacts.Controls.Add(txtDebugConsole);
            tabContacts.Controls.Add(lblDebugConsole);
            tabContacts.Location = new Point(4, 24);
            tabContacts.Name = "tabContacts";
            tabContacts.Padding = new Padding(3);
            tabContacts.Size = new Size(792, 422);
            tabContacts.TabIndex = 1;
            tabContacts.Text = "Contacts";
            tabContacts.UseVisualStyleBackColor = true;
            // 
            // btnSyncContacts
            // 
            btnSyncContacts.Location = new Point(20, 20);
            btnSyncContacts.Name = "btnSyncContacts";
            btnSyncContacts.Size = new Size(120, 29);
            btnSyncContacts.TabIndex = 1;
            btnSyncContacts.Text = "Sync Contacts";
            btnSyncContacts.UseVisualStyleBackColor = true;
            // 
            // lblDebugConsole
            // 
            lblDebugConsole.AutoSize = true;
            lblDebugConsole.Location = new Point(20, 60);
            lblDebugConsole.Name = "lblDebugConsole";
            lblDebugConsole.Size = new Size(90, 15);
            lblDebugConsole.TabIndex = 2;
            lblDebugConsole.Text = "Debug Console:";
            // 
            // txtDebugConsole
            // 
            txtDebugConsole.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtDebugConsole.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtDebugConsole.Location = new Point(20, 80);
            txtDebugConsole.Multiline = true;
            txtDebugConsole.Name = "txtDebugConsole";
            txtDebugConsole.ReadOnly = true;
            txtDebugConsole.ScrollBars = ScrollBars.Vertical;
            txtDebugConsole.Size = new Size(750, 320);
            txtDebugConsole.TabIndex = 3;
            // 
            // tabInvoices
            // 
            tabInvoices.Controls.Add(btnSyncInvoices);
            tabInvoices.Controls.Add(txtInvoicesDebugConsole);
            tabInvoices.Controls.Add(lblInvoicesDebugConsole);
            tabInvoices.Location = new Point(4, 24);
            tabInvoices.Name = "tabInvoices";
            tabInvoices.Padding = new Padding(3);
            tabInvoices.Size = new Size(792, 422);
            tabInvoices.TabIndex = 2;
            tabInvoices.Text = "Invoices";
            tabInvoices.UseVisualStyleBackColor = true;
            // 
            // btnSyncInvoices
            // 
            btnSyncInvoices.Location = new Point(20, 20);
            btnSyncInvoices.Name = "btnSyncInvoices";
            btnSyncInvoices.Size = new Size(120, 29);
            btnSyncInvoices.TabIndex = 1;
            btnSyncInvoices.Text = "Sync Invoices";
            btnSyncInvoices.UseVisualStyleBackColor = true;
            // 
            // lblInvoicesDebugConsole
            // 
            lblInvoicesDebugConsole.AutoSize = true;
            lblInvoicesDebugConsole.Location = new Point(20, 60);
            lblInvoicesDebugConsole.Name = "lblInvoicesDebugConsole";
            lblInvoicesDebugConsole.Size = new Size(90, 15);
            lblInvoicesDebugConsole.TabIndex = 2;
            lblInvoicesDebugConsole.Text = "Debug Console:";
            // 
            // txtInvoicesDebugConsole
            // 
            txtInvoicesDebugConsole.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtInvoicesDebugConsole.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtInvoicesDebugConsole.Location = new Point(20, 80);
            txtInvoicesDebugConsole.Multiline = true;
            txtInvoicesDebugConsole.Name = "txtInvoicesDebugConsole";
            txtInvoicesDebugConsole.ReadOnly = true;
            txtInvoicesDebugConsole.ScrollBars = ScrollBars.Vertical;
            txtInvoicesDebugConsole.Size = new Size(750, 320);
            txtInvoicesDebugConsole.TabIndex = 3;
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
    }
}
