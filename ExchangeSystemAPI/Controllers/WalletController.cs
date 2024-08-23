using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ExchangeSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {

        private readonly WalletService _walletService;
        private readonly UserService _userService;

        public WalletController(WalletService walletService, UserService userService)
        {
            _walletService = walletService;
            _userService = userService;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(int userId, decimal amount)
        {
            var result = await _walletService.DepositAsync(userId, amount);
            if (result)
            {
                return Ok("Deposit successful");
            }
            return BadRequest("Failed to deposit");
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(int userId, decimal amount)
        {
            var result = await _walletService.WithdrawAsync(userId, amount);
            if (result)
            {
                return Ok("Withdrawal successful");
            }
            return BadRequest("Failed to withdraw");
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(int fromUserId, int toUserId, decimal amount)
        {
            var result = await _walletService.TransferAsync(fromUserId, toUserId, amount);
            if (result)
            {
                return Ok("Transfer successful");
            }
            return BadRequest("Failed to transfer");
        }

        [HttpGet("activity-reports")]
        public async Task<IActionResult> GetUserActivityReports()
        {
            var reports = await _userService.GetUserActivityReportsAsync();
            return Ok(reports);
        }

    }
}
