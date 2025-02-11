
using System.Transactions;

namespace Bookify.Domain.Shared
{
    public record Currency
    {
        internal static readonly Currency None = new("");
        public static readonly Currency Eur = new("EUR");
        public static readonly Currency Usd = new("USD");
        private Currency(string code) => Code = code;
        public string Code { get; init; }
        public static Currency FromCode(string code)
        {
            return All.FirstOrDefault(c => c.Code == code) ?? throw new ApplicationException("The curreny code is invalid.");
        }
        public static readonly IReadOnlyCollection<Currency> All = new[] { Eur, Usd };
    }
}