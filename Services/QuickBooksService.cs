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
        private string _companyFile;
        private DateTime _lastConnectionTime;
        private const int CONNECTION_TIMEOUT_MINUTES = 10;

        public QuickBooksService(string companyFile)
        {
            _companyFile = companyFile;
        }

        private bool IsConnectionValid()
        {
            if (!_connectionOpen || _sessionManager == null)
                return false;

            // Check if connection has timed out
            if ((DateTime.Now - _lastConnectionTime).TotalMinutes > CONNECTION_TIMEOUT_MINUTES)
                return false;

            try
            {
                // Try a lightweight operation to verify connection
                var requestMsgSet = _sessionManager.CreateMsgSetRequest("US", 16, 0);
                requestMsgSet.AppendCompanyQueryRq();
                var responseMsgSet = _sessionManager.DoRequests(requestMsgSet);
                return responseMsgSet.ResponseList?.GetAt(0)?.StatusCode == 0;
            }
            catch
            {
                return false;
            }
        }

        private void EnsureValidConnection()
        {
            if (string.IsNullOrEmpty(_companyFile))
            {
                throw new Exception("Company file path is not set.");
            }

            if (IsConnectionValid())
                return;

            // Close existing connection if any
            CloseConnection();

            // Open new connection
            _sessionManager = new QBSessionManager();
            try
            {
                _sessionManager.OpenConnection2("", "CarbonQuickBooks Sync", ENConnectionType.ctLocalQBD);
                _sessionManager.BeginSession(_companyFile, ENOpenMode.omDontCare);
                _connectionOpen = true;
                _lastConnectionTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                CloseConnection();
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
                _lastConnectionTime = DateTime.MinValue;
            }
        }

        public void TestConnection()
        {
            try
            {
                EnsureValidConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddCustomer(Customer customer)
        {
            try
            {
                EnsureValidConnection();

                var requestMsgSet = _sessionManager?.CreateMsgSetRequest("US", 16, 0)
                    ?? throw new InvalidOperationException("Failed to create request message set");

                var customerAdd = requestMsgSet.AppendCustomerAddRq();

                // Set required fields
                customerAdd.Name.SetValue(customer.Name);
                
                // Set optional fields if available
                if (!string.IsNullOrEmpty(customer.Phone))
                {
                    customerAdd.Phone.SetValue(customer.Phone);
                }

                if (!string.IsNullOrEmpty(customer.TaxId))
                {
                    customerAdd.TaxRegistrationNumber.SetValue(customer.TaxId);
                }

                var responseMsgSet = _sessionManager.DoRequests(requestMsgSet);
                var response = responseMsgSet.ResponseList?.GetAt(0);

                if (response?.StatusCode != 0)
                {
                    throw new Exception($"Failed to add customer to QuickBooks: {response?.StatusMessage}");
                }
            }
            catch (Exception)
            {
                CloseConnection();
                throw;
            }
        }

        public void AddSupplier(Supplier supplier)
        {
            try
            {
                EnsureValidConnection();

                var requestMsgSet = _sessionManager?.CreateMsgSetRequest("US", 16, 0)
                    ?? throw new InvalidOperationException("Failed to create request message set");

                var vendorAdd = requestMsgSet.AppendVendorAddRq();

                // Set required fields
                vendorAdd.Name.SetValue(supplier.Name);
                
                // Set optional fields if available
                if (!string.IsNullOrEmpty(supplier.Phone))
                {
                    vendorAdd.Phone.SetValue(supplier.Phone);
                }

                if (!string.IsNullOrEmpty(supplier.TaxId))
                {
                    vendorAdd.TaxRegistrationNumber.SetValue(supplier.TaxId);
                }

                var responseMsgSet = _sessionManager.DoRequests(requestMsgSet);
                var response = responseMsgSet.ResponseList?.GetAt(0);

                if (response?.StatusCode != 0)
                {
                    throw new Exception($"Failed to add vendor to QuickBooks: {response?.StatusMessage}");
                }
            }
            catch (Exception)
            {
                CloseConnection();
                throw;
            }
        }

        public void AddContact(Contact contact)
        {
            try
            {
                EnsureValidConnection();

                var requestMsgSet = _sessionManager?.CreateMsgSetRequest("US", 16, 0)
                    ?? throw new InvalidOperationException("Failed to create request message set");

                if (contact.Type.ToLower() == "customer")
                {
                    var customerAdd = requestMsgSet.AppendCustomerAddRq();
                    customerAdd.Name.SetValue(contact.Name);
                    
                    if (!string.IsNullOrEmpty(contact.CompanyName))
                    {
                        customerAdd.CompanyName.SetValue(contact.CompanyName);
                    }
                    
                    if (!string.IsNullOrEmpty(contact.Email))
                    {
                        customerAdd.Email.SetValue(contact.Email);
                    }
                    
                    if (!string.IsNullOrEmpty(contact.Phone))
                    {
                        customerAdd.Phone.SetValue(contact.Phone);
                    }

                    if (contact.BillingAddress != null)
                    {
                        var billAddress = customerAdd.BillAddress;
                        if (!string.IsNullOrEmpty(contact.BillingAddress.Line1)) billAddress.Addr1.SetValue(contact.BillingAddress.Line1);
                        if (!string.IsNullOrEmpty(contact.BillingAddress.Line2)) billAddress.Addr2.SetValue(contact.BillingAddress.Line2);
                        if (!string.IsNullOrEmpty(contact.BillingAddress.Line3)) billAddress.Addr3.SetValue(contact.BillingAddress.Line3);
                        if (!string.IsNullOrEmpty(contact.BillingAddress.Line4)) billAddress.Addr4.SetValue(contact.BillingAddress.Line4);
                        if (!string.IsNullOrEmpty(contact.BillingAddress.Line5)) billAddress.Addr5.SetValue(contact.BillingAddress.Line5);
                        if (!string.IsNullOrEmpty(contact.BillingAddress.City)) billAddress.City.SetValue(contact.BillingAddress.City);
                        if (!string.IsNullOrEmpty(contact.BillingAddress.State)) billAddress.State.SetValue(contact.BillingAddress.State);
                        if (!string.IsNullOrEmpty(contact.BillingAddress.PostalCode)) billAddress.PostalCode.SetValue(contact.BillingAddress.PostalCode);
                        if (!string.IsNullOrEmpty(contact.BillingAddress.Country)) billAddress.Country.SetValue(contact.BillingAddress.Country);
                    }

                    if (contact.ShippingAddress != null)
                    {
                        var shipAddress = customerAdd.ShipAddress;
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.Line1)) shipAddress.Addr1.SetValue(contact.ShippingAddress.Line1);
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.Line2)) shipAddress.Addr2.SetValue(contact.ShippingAddress.Line2);
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.Line3)) shipAddress.Addr3.SetValue(contact.ShippingAddress.Line3);
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.Line4)) shipAddress.Addr4.SetValue(contact.ShippingAddress.Line4);
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.Line5)) shipAddress.Addr5.SetValue(contact.ShippingAddress.Line5);
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.City)) shipAddress.City.SetValue(contact.ShippingAddress.City);
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.State)) shipAddress.State.SetValue(contact.ShippingAddress.State);
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.PostalCode)) shipAddress.PostalCode.SetValue(contact.ShippingAddress.PostalCode);
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.Country)) shipAddress.Country.SetValue(contact.ShippingAddress.Country);
                    }
                }
                else if (contact.Type.ToLower() == "vendor")
                {
                    var vendorAdd = requestMsgSet.AppendVendorAddRq();
                    vendorAdd.Name.SetValue(contact.Name);
                    
                    if (!string.IsNullOrEmpty(contact.CompanyName))
                    {
                        vendorAdd.CompanyName.SetValue(contact.CompanyName);
                    }
                    
                    if (!string.IsNullOrEmpty(contact.Email))
                    {
                        vendorAdd.Email.SetValue(contact.Email);
                    }
                    
                    if (!string.IsNullOrEmpty(contact.Phone))
                    {
                        vendorAdd.Phone.SetValue(contact.Phone);
                    }

                    if (contact.BillingAddress != null)
                    {
                        var vendorAddress = vendorAdd.VendorAddress;
                        if (!string.IsNullOrEmpty(contact.BillingAddress.Line1)) vendorAddress.Addr1.SetValue(contact.BillingAddress.Line1);
                        if (!string.IsNullOrEmpty(contact.BillingAddress.Line2)) vendorAddress.Addr2.SetValue(contact.BillingAddress.Line2);
                        if (!string.IsNullOrEmpty(contact.BillingAddress.Line3)) vendorAddress.Addr3.SetValue(contact.BillingAddress.Line3);
                        if (!string.IsNullOrEmpty(contact.BillingAddress.Line4)) vendorAddress.Addr4.SetValue(contact.BillingAddress.Line4);
                        if (!string.IsNullOrEmpty(contact.BillingAddress.Line5)) vendorAddress.Addr5.SetValue(contact.BillingAddress.Line5);
                        if (!string.IsNullOrEmpty(contact.BillingAddress.City)) vendorAddress.City.SetValue(contact.BillingAddress.City);
                        if (!string.IsNullOrEmpty(contact.BillingAddress.State)) vendorAddress.State.SetValue(contact.BillingAddress.State);
                        if (!string.IsNullOrEmpty(contact.BillingAddress.PostalCode)) vendorAddress.PostalCode.SetValue(contact.BillingAddress.PostalCode);
                        if (!string.IsNullOrEmpty(contact.BillingAddress.Country)) vendorAddress.Country.SetValue(contact.BillingAddress.Country);
                    }

                    if (contact.ShippingAddress != null)
                    {
                        var shipAddress = vendorAdd.ShipAddress;
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.Line1)) shipAddress.Addr1.SetValue(contact.ShippingAddress.Line1);
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.Line2)) shipAddress.Addr2.SetValue(contact.ShippingAddress.Line2);
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.Line3)) shipAddress.Addr3.SetValue(contact.ShippingAddress.Line3);
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.Line4)) shipAddress.Addr4.SetValue(contact.ShippingAddress.Line4);
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.Line5)) shipAddress.Addr5.SetValue(contact.ShippingAddress.Line5);
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.City)) shipAddress.City.SetValue(contact.ShippingAddress.City);
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.State)) shipAddress.State.SetValue(contact.ShippingAddress.State);
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.PostalCode)) shipAddress.PostalCode.SetValue(contact.ShippingAddress.PostalCode);
                        if (!string.IsNullOrEmpty(contact.ShippingAddress.Country)) shipAddress.Country.SetValue(contact.ShippingAddress.Country);
                    }
                }
                else
                {
                    throw new ArgumentException("Contact type must be either 'Customer' or 'Vendor'");
                }

                var responseMsgSet = _sessionManager.DoRequests(requestMsgSet);
                var response = responseMsgSet.ResponseList?.GetAt(0);

                if (response?.StatusCode != 0)
                {
                    throw new Exception($"Failed to add contact to QuickBooks: {response?.StatusMessage}");
                }
            }
            catch (Exception)
            {
                CloseConnection();
                throw;
            }
        }

        
        public void AddPurchaseOrderInvoice(PurchaseOrder purchaseOrder, List<PurchaseOrderLine> lines, string accountNumber)
        {
            try
            {
                EnsureValidConnection();

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
                    if (!string.IsNullOrEmpty(accountNumber))
                    {
                        billLine.AccountRef.FullName.SetValue(accountNumber);
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
            catch (Exception)
            {
                CloseConnection(); // Close connection on error
                throw;
            }
        }

        public bool ItemExists(string itemName)
        {
            try
            {
                EnsureValidConnection();

                var requestMsgSet = _sessionManager?.CreateMsgSetRequest("US", 16, 0)
                    ?? throw new InvalidOperationException("Failed to create request message set");

                var itemQuery = requestMsgSet.AppendItemQueryRq();
                itemQuery.ORListQuery.FullNameList.Add(itemName);

                var responseMsgSet = _sessionManager.DoRequests(requestMsgSet);
                var response = responseMsgSet.ResponseList?.GetAt(0);

                return response?.StatusCode == 0 && response.Detail != null;
            }
            catch (Exception)
            {
                CloseConnection();
                throw;
            }
        }

        public void AddShipmentInvoiceFromSalesOrder(Shipment shipment, List<ShipmentLine> shipmentLines, SalesOrder salesOrder, List<SalesOrderLine> salesOrderLines, string customerName, string salesAccountName, string expenseAccountName)
        {
            try
            {
                EnsureValidConnection();

                // First ensure all items exist in QuickBooks
                foreach (var shipmentLine in shipmentLines)
                {
                    if (!string.IsNullOrEmpty(shipmentLine.ItemReadableId) && !ItemExists(shipmentLine.ItemReadableId))
                    {
                        // Find corresponding sales order line to get description
                        var salesOrderLine = salesOrderLines.FirstOrDefault(sol => sol.Id == shipmentLine.LineId);
                        
                        // Create the item in QuickBooks
                        var item = new Item
                        {
                            ReadableId = shipmentLine.ItemReadableId,
                            Description = salesOrderLine?.Description ?? shipmentLine.ItemReadableId
                        };
                        AddItem(item, salesAccountName, expenseAccountName);
                    }
                }

                var requestMsgSet = _sessionManager?.CreateMsgSetRequest("US", 16, 0)
                    ?? throw new InvalidOperationException("Failed to create request message set");

                // Create invoice add request
                var invoiceAdd = requestMsgSet.AppendInvoiceAddRq();

                // Set customer reference using the provided QuickBooks customer name
                invoiceAdd.CustomerRef.FullName.SetValue(customerName);

                // Set transaction date to shipment date (using PostingDate instead of ShipmentDate)
                invoiceAdd.TxnDate.SetValue(shipment.PostingDate ?? DateTime.Now);

                // Set reference number (Shipment number)
                invoiceAdd.RefNumber.SetValue(shipment.ShipmentId);

                // Add memo with both SO and Shipment references
                invoiceAdd.Memo.SetValue($"SO: {salesOrder.SalesOrderId}, Shipment: {shipment.ShipmentId}");

                // Add lines based on shipment lines
                foreach (var shipmentLine in shipmentLines)
                {
                    // Find corresponding sales order line
                    var salesOrderLine = salesOrderLines.FirstOrDefault(sol => sol.Id == shipmentLine.LineId);
                    if (salesOrderLine == null) continue;

                    var invoiceLine = invoiceAdd.ORInvoiceLineAddList.Append();
                    var invoiceLineAdd = invoiceLine.InvoiceLineAdd;

                    // Set item reference if provided
                    if (!string.IsNullOrEmpty(shipmentLine.ItemReadableId))
                    {
                        invoiceLineAdd.ItemRef.FullName.SetValue(shipmentLine.ItemReadableId);
                    }

                    // Set description
                    invoiceLineAdd.Desc.SetValue(salesOrderLine.Description ?? shipmentLine.ItemReadableId ?? "");

                    // Set quantity from shipment line (using ShippedQuantity instead of Quantity)
                    invoiceLineAdd.Quantity.SetValue((double)shipmentLine.ShippedQuantity);

                    // Set amount using sales order line price
                    decimal lineAmount = shipmentLine.ShippedQuantity * (salesOrderLine.UnitPrice ?? 0m);
                    invoiceLineAdd.Amount.SetValue((double)lineAmount);

                    // // Only set tax code if tax percent is greater than 0 and we have a valid tax code
                    // if (salesOrderLine.TaxPercent > 0)
                    // {
                    //     try
                    //     {
                    //         // First check if sales tax is enabled by attempting to query tax codes
                    //         var taxRequestMsgSet = _sessionManager.CreateMsgSetRequest("US", 16, 0);
                    //         var taxQuery = taxRequestMsgSet.AppendSalesTaxCodeQueryRq();
                    //         var taxResponseMsgSet = _sessionManager.DoRequests(taxRequestMsgSet);
                    //         var taxResponse = taxResponseMsgSet.ResponseList.GetAt(0);

                    //         // Only set tax code if the query was successful
                    //         if (taxResponse.StatusCode == 0 && taxResponse.Detail != null)
                    //         {
                    //             invoiceLineAdd.SalesTaxCodeRef.FullName.SetValue("Tax");
                    //         }
                    //     }
                    //     catch
                    //     {
                    //         // If there's any error querying tax codes, skip setting the tax code
                    //         // This handles cases where sales tax is disabled or not configured
                    //     }
                    // }
                }

                var responseMsgSet = _sessionManager.DoRequests(requestMsgSet);
                var response = responseMsgSet.ResponseList?.GetAt(0);

                if (response?.StatusCode != 0)
                {
                    throw new Exception($"Failed to add shipment invoice to QuickBooks: {response?.StatusMessage}");
                }
            }
            catch (Exception)
            {
                CloseConnection();
                throw;
            }
        }

        public List<Contact> GetCustomers()
        {
            var customers = new List<Contact>();
            try
            {
                EnsureValidConnection();

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
            catch (Exception)
            {
                CloseConnection(); // Close connection on error
                throw;
            }

            return customers;
        }

        public List<Contact> GetVendors()
        {
            var vendors = new List<Contact>();
            try
            {
                EnsureValidConnection();

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
            catch (Exception)
            {
                CloseConnection(); // Close connection on error
                throw;
            }

            return vendors;
        }

        public List<Invoice> GetInvoices(bool salesInvoices = true)
        {
            var invoices = new List<Invoice>();
            try
            {
                EnsureValidConnection();

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
            catch (Exception)
            {
                CloseConnection(); // Close connection on error
                throw;
            }

            return invoices;
        }

        public bool CustomerExists(string customerName)
        {
            try
            {
                EnsureValidConnection();

                var requestMsgSet = _sessionManager?.CreateMsgSetRequest("US", 16, 0)
                    ?? throw new InvalidOperationException("Failed to create request message set");

                var customerQuery = requestMsgSet.AppendCustomerQueryRq();
                customerQuery.ORCustomerListQuery.FullNameList.Add(customerName);

                var responseMsgSet = _sessionManager.DoRequests(requestMsgSet);
                var response = responseMsgSet.ResponseList?.GetAt(0);

                return response?.StatusCode == 0 && response.Detail != null;
            }
            catch (Exception)
            {
                CloseConnection();
                throw;
            }
        }

        public bool VendorExists(string vendorName)
        {
            try
            {
                EnsureValidConnection();

                var requestMsgSet = _sessionManager?.CreateMsgSetRequest("US", 16, 0)
                    ?? throw new InvalidOperationException("Failed to create request message set");

                var vendorQuery = requestMsgSet.AppendVendorQueryRq();
                vendorQuery.ORVendorListQuery.FullNameList.Add(vendorName);

                var responseMsgSet = _sessionManager.DoRequests(requestMsgSet);
                var response = responseMsgSet.ResponseList?.GetAt(0);

                return response?.StatusCode == 0 && response.Detail != null;
            }
            catch (Exception)
            {
                CloseConnection();
                throw;
            }
        }

        public void AddItem(Item item, string salesAccountName, string expenseAccountName)
        {
            try
            {
                EnsureValidConnection();

                var requestMsgSet = _sessionManager?.CreateMsgSetRequest("US", 16, 0)
                    ?? throw new InvalidOperationException("Failed to create request message set");

                var itemAdd = requestMsgSet.AppendItemNonInventoryAddRq();

                // Set required fields
                itemAdd.Name.SetValue(item.ReadableId);

                // Set optional fields
                if (!string.IsNullOrEmpty(item.Description))
                {
                    itemAdd.ORSalesPurchase.SalesAndPurchase.SalesDesc.SetValue(item.Description);
                    itemAdd.ORSalesPurchase.SalesAndPurchase.PurchaseDesc.SetValue(item.Description);
                }

                // Set account references from configuration
                itemAdd.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef.FullName.SetValue(salesAccountName);
                itemAdd.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef.FullName.SetValue(expenseAccountName);

                var responseMsgSet = _sessionManager.DoRequests(requestMsgSet);
                var response = responseMsgSet.ResponseList?.GetAt(0);

                if (response?.StatusCode != 0)
                {
                    throw new Exception($"Failed to add item to QuickBooks: {response?.StatusMessage}");
                }
            }
            catch (Exception)
            {
                CloseConnection();
                throw;
            }
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