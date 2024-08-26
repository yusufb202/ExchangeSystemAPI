using Core.DTOS;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Security.Claims;

namespace ExchangeSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeTransactionsController : ControllerBase
    {
        private readonly ExchangeDbContext _context;
        private readonly UserManager<User> _userManager;

        public ExchangeTransactionsController(ExchangeDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Policy = "User")]
        [HttpPost]
        public async Task<IActionResult> PostExchangeTransaction([FromBody] ExchangeTransactionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
                return NotFound("User not found");

            var fromCurrency = await _context.Currencies.FirstOrDefaultAsync(c => c.Code == dto.FromCurrencyCode);
            var toCurrency = await _context.Currencies.FirstOrDefaultAsync(c => c.Code == dto.ToCurrencyCode);

            if (fromCurrency == null || toCurrency == null)
                return NotFound("Currency not found");

            var transaction = new ExchangeTransaction
            {
                UserId = user.Id,
                FromCurrencyId = fromCurrency.Id,
                ToCurrencyId = toCurrency.Id,
                FromCurrencyName = fromCurrency.Name,
                ToCurrencyName = toCurrency.Name,
                ExchangeRate = dto.ExchangeRate,
                Amount = dto.Amount,
                TransactionDate = dto.TransactionDate
            };

            _context.ExchangeTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(transaction);  // Simplified response without GetTransactionById
        }

        [Authorize(Policy = "User")]
        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<ExchangeTransaction>>> GetTransactionHistory()
        {
            var transactions = await _context.ExchangeTransactions
                .Include(t => t.FromCurrency)
                .Include(t => t.ToCurrency)
                .ToListAsync();

            return Ok(transactions);
        }
    }
}
