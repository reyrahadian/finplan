﻿using FinPlan.Domain;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Currency
{
	public class GetCurrenciesRequest : IRequest<IEnumerable<Currency>>
	{
	}

	public class GetCurrenciesRequestHandler : IRequestHandler<GetCurrenciesRequest, IEnumerable<Currency>>
	{
		public Task<IEnumerable<Currency>> Handle(GetCurrenciesRequest request, CancellationToken cancellationToken)
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
