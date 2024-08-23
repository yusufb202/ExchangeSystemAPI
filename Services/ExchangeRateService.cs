using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ExchangeRateService
    {
        private readonly IRepository<ExchangeRate> _exchangeRateRepository;

        public ExchangeRateService(IRepository<ExchangeRate> exchangeRateRepository)
        {
            _exchangeRateRepository = exchangeRateRepository;
        }

        public async Task<List<ExchangeRate>> GetExchangeRateTrendsAsync(string fromCurrency, string toCurrency, DateTime startDate, DateTime endDate)
        {
            var exchangeRates = await _exchangeRateRepository.GetAllAsync();
            return exchangeRates
                .Where(er => er.FromCurrency.Code == fromCurrency && er.ToCurrency.Code == toCurrency && er.Date >= startDate && er.Date <= endDate)
                .ToList();
        }
    }
}
