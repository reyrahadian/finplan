using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FinPlan.Domain
{
	public class CurrencyService
	{
		public IEnumerable<Currency> GetCurrencies()
		{
			var currencies = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
				.Select(ci => ci.LCID).Distinct()
				.Select(id => new RegionInfo(id))
				.GroupBy(r => r.ISOCurrencySymbol)
				.Select(g => g.First())
				.Select(r => new Currency
				{
					ISOCurrencySymbol = r.ISOCurrencySymbol,
					EnglishName = r.CurrencyEnglishName,
					Symbol = r.CurrencySymbol
				});

			return currencies;
		}
	}
}