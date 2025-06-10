namespace CarbonQuickBooks.Models
{
    public class Contact
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Address? BillingAddress { get; set; }
        public Address? ShippingAddress { get; set; }
    }

    public class Address
    {
        public string Line1 { get; set; } = string.Empty;
        public string Line2 { get; set; } = string.Empty;
        public string Line3 { get; set; } = string.Empty;
        public string Line4 { get; set; } = string.Empty;
        public string Line5 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        
        public override string ToString()
        {
            var lines = new List<string>();
            if (!string.IsNullOrEmpty(Line1)) lines.Add(Line1);
            if (!string.IsNullOrEmpty(Line2)) lines.Add(Line2);
            if (!string.IsNullOrEmpty(Line3)) lines.Add(Line3);
            if (!string.IsNullOrEmpty(Line4)) lines.Add(Line4);
            if (!string.IsNullOrEmpty(Line5)) lines.Add(Line5);
            
            var cityStateZip = new List<string>();
            if (!string.IsNullOrEmpty(City)) cityStateZip.Add(City);
            if (!string.IsNullOrEmpty(State)) cityStateZip.Add(State);
            if (!string.IsNullOrEmpty(PostalCode)) cityStateZip.Add(PostalCode);
            
            if (cityStateZip.Count > 0) lines.Add(string.Join(", ", cityStateZip));
            if (!string.IsNullOrEmpty(Country)) lines.Add(Country);
            
            return string.Join("\n    ", lines);
        }
    }
}