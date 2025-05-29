using System.Runtime.InteropServices;
using CarbonQuickBooks.Models;
using Interop.QBFC16;


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
                                customers.Add(new Contact
                                {
                                    Name = customerRet.FullName.GetValue(),
                                    Type = "Customer",
                                    Email = customerRet.Email?.GetValue() ?? string.Empty,
                                    Phone = customerRet.Phone?.GetValue() ?? string.Empty
                                });
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
                                vendors.Add(new Contact
                                {
                                    Name = vendorRet.Name.GetValue(),
                                    Type = "Vendor",
                                    Email = vendorRet.Email?.GetValue() ?? string.Empty,
                                    Phone = vendorRet.Phone?.GetValue() ?? string.Empty
                                });
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