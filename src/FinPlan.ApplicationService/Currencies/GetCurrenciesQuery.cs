using FinPlan.Domain.Currencies;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Currencies
{
	public class GetCurrenciesQuery : IRequest<IEnumerable<Currency>>
	{
	}

	public class GetCurrenciesRequestHandler : IRequestHandler<GetCurrenciesQuery, IEnumerable<Currency>>
	{
		public Task<IEnumerable<Currency>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
		{
			var currencyService = new CurrencyService();
			return Task.FromResult(currencyService.GetCurrencies().Select(x => new Currency
			{
				Symbol = x.Symbol,
				EnglishName = x.EnglishName,
				ISOCurrencySymbol = x.ISOCurrencySymbol
			}).OrderBy(x => x.EnglishName).AsEnumerable());
		}
	}
}
