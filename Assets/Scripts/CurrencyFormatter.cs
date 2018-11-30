using System.Globalization;

public class CurrencyFormatter {
    public static string Format(int price) {
        return price.ToString("C0", CultureInfo.CreateSpecificCulture("en-GB"));
    }
}
