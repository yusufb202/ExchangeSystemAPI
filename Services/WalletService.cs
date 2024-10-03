using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class WalletService
    {
        private readonly IRepository<Wallet> _walletRepository;
        private readonly ExchangeRateService _exchangeRateService;
        private readonly IRepository<ExchangeRate> _exchangeRateRepository;

        public WalletService(IRepository<Wallet> walletRepository, ExchangeRateService exchangeRateService, IRepository<ExchangeRate> exchangeRateRepository)
        {
            _walletRepository = walletRepository;
            _exchangeRateService = exchangeRateService;
            _exchangeRateRepository = exchangeRateRepository;
        }

        public async Task<bool> DepositAsync(int userId, decimal amount)
        {
            var wallet = await _walletRepository.FindAsync(x => x.UserId == userId);
            if (wallet == null)
            {
                return false;
            }

            wallet.Balance += amount;
            await _walletRepository.UpdateAsync(wallet);
            return true;
        }

        public async Task<bool> WithdrawAsync(int userId, decimal amount)
        {
            var wallet = await _walletRepository.FindAsync(x => x.UserId == userId);
            if (wallet == null || wallet.Balance < amount) return false;

            wallet.Balance -= amount;
            await _walletRepository.UpdateAsync(wallet);

            return true;
        }

        public async Task<bool> TransferAsync(int fromUserId, int toUserId, decimal amount)
        {
            var fromWallet = await _walletRepository.FindAsync(x => x.UserId == fromUserId);
            var toWallet = await _walletRepository.FindAsync(x => x.UserId == toUserId);

            if (fromWallet == null || toWallet == null || fromWallet.Balance < amount)
            {
                return false;
            }

            fromWallet.Balance -= amount;
            toWallet.Balance += amount;

            await _walletRepository.UpdateAsync(fromWallet);
            await _walletRepository.UpdateAsync(toWallet);

            return true;
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
