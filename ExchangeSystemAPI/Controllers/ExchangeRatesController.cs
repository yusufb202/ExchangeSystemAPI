using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace ExchangeSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly ExchangeDbContext _context;

        public ExchangeRatesController(ExchangeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExchangeRate>>> GetExchangeRates()
        {
            return await _context.ExchangeRates
                .Include(er => er.FromCurrency)
                .Include(er => er.ToCurrency)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExchangeRate>> GetExchangeRate(int id)
        {
            var exchangeRate= await _context.ExchangeRates
                .Include(er => er.FromCurrency)
                .Include(er => er.ToCurrency)
                .FirstOrDefaultAsync(er=>er.Id==id);

            if(exchangeRate == null)
            {
                return NotFound();
            }

            return Ok(exchangeRate);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> PutExchangeRate(int id, ExchangeRate exchangeRate)
        {
            if(id!=exchangeRate.Id)
            {
                return BadRequest();
            }
            _context.Entry(exchangeRate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }

            catch(DbUpdateConcurrencyException)
            {
                if (!ExchangeRateExists(id))
                {
                    return NotFound();
                }

                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ExchangeRate>> PostExchangeRate(ExchangeRate exchangeRate)
        {
            _context.ExchangeRates.Add(exchangeRate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExchangeRate", new { id = exchangeRate.Id }, exchangeRate);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteExchangeRate(int id)
        {
            var exchangeRate = await _context.ExchangeRates.FindAsync(id);
            if(exchangeRate == null)
            {
                return NotFound();
            }

            _context.ExchangeRates.Remove(exchangeRate);

            await _context.SaveChangesAsync();

            return NoContent();

        }

        private bool ExchangeRateExists(int id)
        {
            return _context.ExchangeRates.Any(e=>e.Id == id);
        }
    }
}
