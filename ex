public class Purchase
{
    public double Price { get; set; }
    public int Quantity { get; set; }
    public PaymentType PaymentType { get; set; }

    public double PayTax(double price)
    {
        double taxRate = 0.04; // Default tax rate

        switch (PaymentType)
        {
            case PaymentType.DebitCard:
                taxRate = 0.02;
                break;
            case PaymentType.CreditCard:
                taxRate = 0.03;
                break;
            case PaymentType.Cash:
                taxRate = 0.01;
                break;
        }

        double total = price * Quantity * (1 + taxRate);
        return total;
    }
}

public enum PaymentType
{
    DebitCard,
    CreditCard,
    Cash
}

public class InternationalSeller : Purchase
{
    public double ExportCharge { get; set; }
    public string[] SellerLocations { get; set; }

    public override double PayTax(double price) // Override to include international tax
    {
        double totalTax = base.PayTax(price); // Call base class PayTax for purchase tax
        totalTax += price * GetLocationTaxRate();

        return price + ExportCharge + totalTax;
    }

    private double GetLocationTaxRate()
    {
        double taxRate = 0;

        foreach (var location in SellerLocations)
        {
            switch (location.ToUpper()) // Case-sensitive comparison
            {
                case "MIDDLE EAST":
                    taxRate += 0.15;
                    break;
                case "EUROPE":
                    taxRate += 0.25;
                    break;
                case "CANADA":
                    taxRate += 0.22;
                    break;
                case "JAPAN":
                    taxRate += 0.12;
                    break;
            }
        }

        return taxRate;
    }
}

public class TaxDemo // Optional Usage Example
{
    public static void Main(string[] args)
    {
        // Purchase Example
        Purchase purchase1 = new Purchase();
        purchase1.Price = 10.00;
        purchase1.Quantity = 2;
        purchase1.PaymentType = PaymentType.DebitCard;

        double totalWithTax1 = purchase1.PayTax(purchase1.Price);
        Console.WriteLine($"Purchase with Debit Card (2 items): ${totalWithTax1:F2}");

        // International Seller Example
        InternationalSeller seller = new InternationalSeller();
        seller.Price = 50.00;
        seller.ExportCharge = 15.00;
        seller.Quantity = 1; // Assuming quantity applies to international seller too
        seller.PaymentType = PaymentType.CreditCard; // Set payment type for purchase tax
        seller.SellerLocations = new string[] { "Europe", "Canada" };

        double totalWithTax2 = seller.PayTax(seller.Price);
        Console.WriteLine($"International Seller (Europe & Canada): ${totalWithTax2:F2}");
    }
}
