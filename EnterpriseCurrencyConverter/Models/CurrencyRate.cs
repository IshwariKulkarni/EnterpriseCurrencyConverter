using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseCurrencyConverter.Models
{
    /// <summary>
    /// MODEL: Represents a single currency with its exchange rate.
    /// This is a plain C# class — no UI knowledge whatsoever.
    /// The Model is your "data shape".
    /// </summary>
    public class CurrencyRate
    {
        // ISO 4217 currency code (e.g., "INR", "EUR", "GBP")
        public string CurrencyCode { get; set; } = string.Empty;

        // Human-readable name shown in the ComboBox
        public string CurrencyName { get; set; } = string.Empty;

        // How many units of this currency equal 1 USD
        // Example: If 1 USD = 83.5 INR, then Rate = 83.5
        public decimal Rate { get; set; }

        // Flag emoji for visual appeal — great for enterprise UI!
        public string Flag { get; set; } = string.Empty;

        // This is what ComboBox displays for each item
        public override string ToString()
        {
            return $"{Flag}  {CurrencyCode} — {CurrencyName}";
        }
    }
}
