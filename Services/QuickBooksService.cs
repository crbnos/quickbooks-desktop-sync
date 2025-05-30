using System.Runtime.InteropServices;
using CarbonQuickBooks.Models;
using Interop.QBFC16;
using Models;

namespace CarbonQuickBooks.Services
{
    public class QuickBooksService : IDisposable
    {
        private QBSessionManager? _sessionManager;
        private bool _connectionOpen;
        private readonly string _companyFile;

        public QuickBooksService(string companyFile)
        {
            _companyFile = companyFile;
        }

        public void TestConnection()
        {
            if (string.IsNullOrEmpty(_companyFile))
            {
                throw new Exception("Company file path is not set.");
            }

            try
            {
                OpenConnection();
                // Try to create a simple request to verify the connection is working
                var requestMsgSet = _sessionManager?.CreateMsgSetRequest("US", 16, 0)
                    ?? throw new InvalidOperationException("Failed to create request message set");
                
                // Append a simple query (CompanyQuery is lightweight)
                requestMsgSet.AppendCompanyQueryRq();
                
                // Execute the request
                var responseMsgSet = _sessionManager.DoRequests(requestMsgSet);
                var response = responseMsgSet.ResponseList?.GetAt(0);

                if (response?.StatusCode != 0)
                {
                    throw new Exception($"QuickBooks error: {response?.StatusMessage}");
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public void AddSalesOrderInvoice(SalesOrder salesOrder, List<SalesOrderLine> lines)
        {
            OpenConnection();

            try
            {
                var requestMsgSet = _sessionManager?.CreateMsgSetRequest("US", 16, 0)
                    ?? throw new InvalidOperationException("Failed to create request message set");

                // Create invoice add request
                var invoiceAdd = requestMsgSet.AppendInvoiceAddRq();

                // Set customer reference
                invoiceAdd.CustomerRef.FullName.SetValue(salesOrder.CustomerId);

                // Set transaction date
                invoiceAdd.TxnDate.SetValue(salesOrder.OrderDate);

                // Set reference number (SO number)
                invoiceAdd.RefNumber.SetValue(salesOrder.SalesOrderId);

                // Add memo
                invoiceAdd.Memo.SetValue($"SO: {salesOrder.SalesOrderId}");

                // Add lines
                foreach (var line in lines)
                {
                    var invoiceLine = invoiceAdd.ORInvoiceLineAddList.Append();
                    var invoiceLineAdd = invoiceLine.InvoiceLineAdd;

                    // Set item reference if provided
                    if (!string.IsNullOrEmpty(line.ItemId))
                    {
                        invoiceLineAdd.ItemRef.FullName.SetValue(line.ItemId);
                    }

                    // Set description
                    invoiceLineAdd.Desc.SetValue(line.Description ?? line.ItemReadableId ?? "");

                    // Set quantity
                    invoiceLineAdd.Quantity.SetValue((double)(line.QuantityToInvoice ?? 0));


                    // Set amount
                    decimal lineAmount = (line.QuantityToInvoice ?? 0) * (line.UnitPrice ?? 0);
                    invoiceLineAdd.Amount.SetValue((double)lineAmount);

                    // Set tax info if applicable
                    if (line.TaxPercent > 0)
                    {
                        invoiceLineAdd.SalesTaxCodeRef.FullName.SetValue("Tax");
                    }
                }

                var responseMsgSet = _sessionManager.DoRequests(requestMsgSet);
                var response = responseMsgSet.ResponseList?.GetAt(0);

                if (response?.StatusCode != 0)
                {
                    throw new Exception($"Failed to add invoice to QuickBooks: {response?.StatusMessage}");
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public void AddPurchaseOrderInvoice(PurchaseOrder purchaseOrder, List<PurchaseOrderLine> lines)
        {
            OpenConnection();

            try
            {
                var requestMsgSet = _sessionManager?.CreateMsgSetRequest("US", 16, 0)
                    ?? throw new InvalidOperationException("Failed to create request message set");

                // Create bill add request
                var billAdd = requestMsgSet.AppendBillAddRq();

                // Set vendor reference
                billAdd.VendorRef.FullName.SetValue(purchaseOrder.SupplierId);

                // Set transaction date
                billAdd.TxnDate.SetValue(purchaseOrder.OrderDate ?? DateTime.Now);

                // Set reference number (PO number)
                billAdd.RefNumber.SetValue(purchaseOrder.PurchaseOrderId);

                // Add memo
                billAdd.Memo.SetValue($"PO: {purchaseOrder.PurchaseOrderId}");

                // Add lines
                foreach (var line in lines)
                {
                    var billLine = billAdd.ExpenseLineAddList.Append();

                    // Set account number if provided
                    if (!string.IsNullOrEmpty(line.AccountNumber))
                    {
                        billLine.AccountRef.FullName.SetValue(line.AccountNumber);
                    }

                    

                    // Set amount
                    decimal lineAmount = (line.QuantityToInvoice ?? 0) * (line.UnitPrice ?? 0);
                    billLine.Amount.SetValue((double)lineAmount);

                    // Set tax amount if applicable
                    if (line.TaxAmount > 0)
                    {
                        billLine.TaxAmount.SetValue((double)line.TaxAmount);
                    }
                }

                var responseMsgSet = _sessionManager.DoRequests(requestMsgSet);
                var response = responseMsgSet.ResponseList?.GetAt(0);

                if (response?.StatusCode != 0)
                {
                    throw new Exception($"Failed to add bill to QuickBooks: {response?.StatusMessage}");
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        private void OpenConnection()
        {
            if (_connectionOpen) return;

            _sessionManager = new QBSessionManager();
            try
            {
                _sessionManager.OpenConnection2("", "CarbonQuickBooks Sync", ENConnectionType.ctLocalQBD);
                _sessionManager.BeginSession(_companyFile, ENOpenMode.omDontCare);
                _connectionOpen = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to connect to QuickBooks: " + ex.Message);
            }
        }

        private void CloseConnection()
        {
            if (!_connectionOpen) return;

            try
            {
                if (_sessionManager != null)
                {
                    _sessionManager.EndSession();
                    _sessionManager.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to close QuickBooks connection: " + ex.Message);
            }
            finally
            {
                _connectionOpen = false;
            }
        }

        public List<Contact> GetCustomers()
        {
            var customers = new List<Contact>();
            OpenConnection();

            try
            {
                var requestMsgSet = _sessionManager?.CreateMsgSetRequest("US", 16, 0)
                    ?? throw new InvalidOperationException("Failed to create request message set");

                var customerQuery = requestMsgSet.AppendCustomerQueryRq();
                var responseMsgSet = _sessionManager.DoRequests(requestMsgSet);
                var response = responseMsgSet.ResponseList?.GetAt(0);

                if (response?.StatusCode == 0)
                {
                    if (response.Detail is ICustomerRetList customerRetList)
                    {
                        for (int i = 0; i < customerRetList.Count; i++)
                        {
                            var customerRet = customerRetList.GetAt(i);
                            if (customerRet?.FullName?.GetValue() != null)
                            {
                                var customer = new Contact
                                {
                                    Name = customerRet.FullName.GetValue(),
                                    CompanyName = customerRet.CompanyName?.GetValue() ?? string.Empty,
                                    Type = "Customer",
                                    Email = customerRet.Email?.GetValue() ?? string.Empty,
                                    Phone = customerRet.Phone?.GetValue() ?? string.Empty
                                };

                                // Get billing address
                                if (customerRet.BillAddress != null)
                                {
                                    customer.BillingAddress = new Address
                                    {
                                        Line1 = customerRet.BillAddress.Addr1?.GetValue() ?? string.Empty,
                                        Line2 = customerRet.BillAddress.Addr2?.GetValue() ?? string.Empty,
                                        Line3 = customerRet.BillAddress.Addr3?.GetValue() ?? string.Empty,
                                        Line4 = customerRet.BillAddress.Addr4?.GetValue() ?? string.Empty,
                                        Line5 = customerRet.BillAddress.Addr5?.GetValue() ?? string.Empty,
                                        City = customerRet.BillAddress.City?.GetValue() ?? string.Empty,
                                        State = customerRet.BillAddress.State?.GetValue() ?? string.Empty,
                                        PostalCode = customerRet.BillAddress.PostalCode?.GetValue() ?? string.Empty,
                                        Country = customerRet.BillAddress.Country?.GetValue() ?? string.Empty
                                    };
                                }

                                // Get shipping address
                                if (customerRet.ShipAddress != null)
                                {
                                    customer.ShippingAddress = new Address
                                    {
                                        Line1 = customerRet.ShipAddress.Addr1?.GetValue() ?? string.Empty,
                                        Line2 = customerRet.ShipAddress.Addr2?.GetValue() ?? string.Empty,
                                        Line3 = customerRet.ShipAddress.Addr3?.GetValue() ?? string.Empty,
                                        Line4 = customerRet.ShipAddress.Addr4?.GetValue() ?? string.Empty,
                                        Line5 = customerRet.ShipAddress.Addr5?.GetValue() ?? string.Empty,
                                        City = customerRet.ShipAddress.City?.GetValue() ?? string.Empty,
                                        State = customerRet.ShipAddress.State?.GetValue() ?? string.Empty,
                                        PostalCode = customerRet.ShipAddress.PostalCode?.GetValue() ?? string.Empty,
                                        Country = customerRet.ShipAddress.Country?.GetValue() ?? string.Empty
                                    };
                                }

                                customers.Add(customer);
                            }
                        }
                    }
                }
            }
            finally
            {
                CloseConnection();
            }

            return customers;
        }

        public List<Contact> GetVendors()
        {
            var vendors = new List<Contact>();
            OpenConnection();

            try
            {
                var requestMsgSet = _sessionManager?.CreateMsgSetRequest("US", 16, 0)
                    ?? throw new InvalidOperationException("Failed to create request message set");

                var vendorQuery = requestMsgSet.AppendVendorQueryRq();
                var responseMsgSet = _sessionManager.DoRequests(requestMsgSet);
                var response = responseMsgSet.ResponseList?.GetAt(0);

                if (response?.StatusCode == 0)
                {
                    if (response.Detail is IVendorRetList vendorRetList)
                    {
                        for (int i = 0; i < vendorRetList.Count; i++)
                        {
                            var vendorRet = vendorRetList.GetAt(i);
                            if (vendorRet?.Name?.GetValue() != null)
                            {
                                var vendor = new Contact
                                {
                                    Name = vendorRet.Name.GetValue(),
                                    CompanyName = vendorRet.CompanyName?.GetValue() ?? string.Empty,
                                    Type = "Vendor",
                                    Email = vendorRet.Email?.GetValue() ?? string.Empty,
                                    Phone = vendorRet.Phone?.GetValue() ?? string.Empty
                                };

                                // Get billing address
                                if (vendorRet.VendorAddress != null)
                                {
                                    vendor.BillingAddress = new Address
                                    {
                                        Line1 = vendorRet.VendorAddress.Addr1?.GetValue() ?? string.Empty,
                                        Line2 = vendorRet.VendorAddress.Addr2?.GetValue() ?? string.Empty,
                                        Line3 = vendorRet.VendorAddress.Addr3?.GetValue() ?? string.Empty,
                                        Line4 = vendorRet.VendorAddress.Addr4?.GetValue() ?? string.Empty,
                                        Line5 = vendorRet.VendorAddress.Addr5?.GetValue() ?? string.Empty,
                                        City = vendorRet.VendorAddress.City?.GetValue() ?? string.Empty,
                                        State = vendorRet.VendorAddress.State?.GetValue() ?? string.Empty,
                                        PostalCode = vendorRet.VendorAddress.PostalCode?.GetValue() ?? string.Empty,
                                        Country = vendorRet.VendorAddress.Country?.GetValue() ?? string.Empty
                                    };
                                }

                                // Get shipping address
                                if (vendorRet.ShipAddress != null)
                                {
                                    vendor.ShippingAddress = new Address
                                    {
                                        Line1 = vendorRet.ShipAddress.Addr1?.GetValue() ?? string.Empty,
                                        Line2 = vendorRet.ShipAddress.Addr2?.GetValue() ?? string.Empty,
                                        Line3 = vendorRet.ShipAddress.Addr3?.GetValue() ?? string.Empty,
                                        Line4 = vendorRet.ShipAddress.Addr4?.GetValue() ?? string.Empty,
                                        Line5 = vendorRet.ShipAddress.Addr5?.GetValue() ?? string.Empty,
                                        City = vendorRet.ShipAddress.City?.GetValue() ?? string.Empty,
                                        State = vendorRet.ShipAddress.State?.GetValue() ?? string.Empty,
                                        PostalCode = vendorRet.ShipAddress.PostalCode?.GetValue() ?? string.Empty,
                                        Country = vendorRet.ShipAddress.Country?.GetValue() ?? string.Empty
                                    };
                                }

                                vendors.Add(vendor);
                            }
                        }
                    }
                }
            }
            finally
            {
                CloseConnection();
            }

            return vendors;
        }

        public List<Invoice> GetInvoices(bool salesInvoices = true)
        {
            var invoices = new List<Invoice>();
            OpenConnection();

            try
            {
                var requestMsgSet = _sessionManager?.CreateMsgSetRequest("US", 16, 0)
                    ?? throw new InvalidOperationException("Failed to create request message set");

                if (salesInvoices)
                {
                    _ = requestMsgSet.AppendInvoiceQueryRq();
                }
                else
                {
                    _ = requestMsgSet.AppendBillQueryRq();
                }

                var responseMsgSet = _sessionManager.DoRequests(requestMsgSet);
                var response = responseMsgSet.ResponseList?.GetAt(0);

                if (response?.StatusCode == 0)
                {
                    if (salesInvoices)
                    {
                        if (response.Detail is IInvoiceRetList invoiceRetList)
                        {
                            for (int i = 0; i < invoiceRetList.Count; i++)
                            {
                                var invoiceRet = invoiceRetList.GetAt(i);
                                if (invoiceRet?.RefNumber?.GetValue() != null)
                                {
                                    invoices.Add(new Invoice
                                    {
                                        InvoiceNumber = invoiceRet.RefNumber.GetValue(),
                                        Date = invoiceRet.TxnDate?.GetValue() ?? DateTime.MinValue,
                                        Type = "Sales Invoice",
                                        ContactName = invoiceRet.CustomerRef?.FullName?.GetValue() ?? string.Empty,
                                        Amount = (decimal)(invoiceRet.BalanceRemaining?.GetValue() ?? 0), // Updated to use BalanceRemaining
                                        Status = "Pending"
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        if (response.Detail is IBillRetList billRetList)
                        {
                            for (int i = 0; i < billRetList.Count; i++)
                            {
                                var billRet = billRetList.GetAt(i);
                                if (billRet?.RefNumber?.GetValue() != null)
                                {
                                    invoices.Add(new Invoice
                                    {
                                        InvoiceNumber = billRet.RefNumber.GetValue(),
                                        Date = billRet.TxnDate?.GetValue() ?? DateTime.MinValue,
                                        Type = "Purchase Invoice",
                                        ContactName = billRet.VendorRef?.FullName?.GetValue() ?? string.Empty,
                                        Amount = (decimal)(billRet.OpenAmount?.GetValue() ?? 0), // Updated to cast double to decimal
                                        Status = "Pending"
                                    });
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                CloseConnection();
            }

            return invoices;
        }

        public void Dispose()
        {
            CloseConnection();
            if (_sessionManager != null)
            {
                Marshal.ReleaseComObject(_sessionManager);
            }
        }
    }
}