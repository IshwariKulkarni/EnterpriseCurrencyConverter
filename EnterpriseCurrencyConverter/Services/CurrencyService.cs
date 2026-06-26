using EnterpriseCurrencyConverter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseCurrencyConverter.Services
{
    /// <summary>
    /// SERVICE: Contains business logic for currency conversion.
    /// 
    /// In a real enterprise app, this would call a REST API (like Open Exchange Rates).
    /// For our demo, we use hardcoded rates — but the architecture is identical.
    /// 
    /// Services are injected into ViewModels (Dependency Injection concept).
    /// This makes the ViewModel testable without needing live API calls.
    /// </summary>
    public class CurrencyService
    {
        /// <summary>
        /// Returns all supported currencies with their USD exchange rates.
        /// In production: replace this with an HttpClient call to a rates API.
        /// </summary>
        public List<CurrencyRate> GetSupportedCurrencies()
        {
            // Rates as of approximate 2024 values — hardcoded for the demo
            return new List<CurrencyRate>
            {
                new CurrencyRate { CurrencyCode = "INR", CurrencyName = "Indian Rupee",       Rate = 83.50m,   Flag = "🇮🇳" },
                new CurrencyRate { CurrencyCode = "EUR", CurrencyName = "Euro",                Rate = 0.92m,    Flag = "🇪🇺" },
                new CurrencyRate { CurrencyCode = "GBP", CurrencyName = "British Pound",       Rate = 0.79m,    Flag = "🇬🇧" },
                new CurrencyRate { CurrencyCode = "JPY", CurrencyName = "Japanese Yen",        Rate = 149.50m,  Flag = "🇯🇵" },
                new CurrencyRate { CurrencyCode = "CAD", CurrencyName = "Canadian Dollar",     Rate = 1.36m,    Flag = "🇨🇦" },
                new CurrencyRate { CurrencyCode = "AUD", CurrencyName = "Australian Dollar",   Rate = 1.53m,    Flag = "🇦🇺" },
                new CurrencyRate { CurrencyCode = "CHF", CurrencyName = "Swiss Franc",         Rate = 0.90m,    Flag = "🇨🇭" },
                new CurrencyRate { CurrencyCode = "CNY", CurrencyName = "Chinese Yuan",        Rate = 7.24m,    Flag = "🇨🇳" },
                new CurrencyRate { CurrencyCode = "SAR", CurrencyName = "Saudi Riyal",         Rate = 3.75m,    Flag = "🇸🇦" },
                new CurrencyRate { CurrencyCode = "AED", CurrencyName = "UAE Dirham",          Rate = 3.67m,    Flag = "🇦🇪" },
            };
        }

        /// <summary>
        /// Core conversion formula:
        ///   Result = Amount (in USD) × Target Currency Rate
        /// 
        /// Example: Convert $100 to INR
        ///   Result = 100 × 83.50 = ₹8,350
        /// </summary>
        public decimal Convert(decimal amountInUsd, CurrencyRate targetCurrency)
        {
            return amountInUsd * targetCurrency.Rate;
        }
    }
}
