using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService
    {
        private readonly IRepository<UserActivityReport> _userActivityReportRepository;
        private readonly IRepository<ExchangeTransaction> _exchangeTransactionRepository;

        public UserService(IRepository<UserActivityReport> userActivityReportRepository, IRepository<ExchangeTransaction> exchangeTransactionRepository)
        {
            _userActivityReportRepository = userActivityReportRepository;
            _exchangeTransactionRepository = exchangeTransactionRepository;
        }

        public async Task<List<UserActivityReport>> GetUserActivityReportsAsync(int userId)
        {
            var allTransactions = await _exchangeTransactionRepository.GetAllAsync(); // Get all transactions
            var transactions = allTransactions.Where(t => t.UserId == userId).ToList(); // Filter by userId

            // Generate the report, such as counting transactions, summing amounts, etc.
            var report = new UserActivityReport
            {
                UserId = userId,
                TotalTransactions = transactions.Count(), // Ensure System.Linq is included
                TotalAmountExchanged = transactions.Sum(t => t.Amount), // Ensure System.Linq is included
                MostTradedCurrency = transactions.Where(t => t.ToCurrency != null)
                                                 .GroupBy(t => t.ToCurrency.Code) // Ensure System.Linq is included
                                                 .OrderByDescending(g => g.Count())
                                                 .Select(g => g.Key)
                                                 .FirstOrDefault() ?? string.Empty // Handle possible null reference
            };

            return new List<UserActivityReport> { report };
        }
    }

}
