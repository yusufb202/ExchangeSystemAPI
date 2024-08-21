using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExchangeSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ExchangeDbContext _dbContext;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, ExchangeDbContext dbContext)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Create the new user
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                // Reload the created user from the database
                var createdUser = await _userManager.FindByNameAsync(user.UserName);
                if (createdUser == null)
                {
                    return BadRequest("User creation failed.");
                }

                // Assign the User role
                var roleResult = await _userManager.AddToRoleAsync(createdUser, "User");
                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                }

                // Generate and assign WalletId
                var walletId = GenerateWalletId();
                var wallet = new Wallet
                {
                    WalletId = walletId,
                    UserId = createdUser.Id,
                    Balance = 0
                };

                _dbContext.Wallets.Add(wallet);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new { WalletId = walletId });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log the exception for debugging
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized();

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return BadRequest("User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Auth", new { token, email = user.Email }, Request.Scheme);

            //Send resetLink via email (Implement email sending logic here)


            // Send email with callbackUrl
            return Ok("Password reset link has been sent to your email.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return BadRequest("User not found");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password has been reset successfully.");
        }

        [HttpPost("enable-2fa")]

        public async Task<IActionResult> Enable2fa()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return BadRequest("User not found");

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Authenticator");

            //display token to user and ask for it to enable 2fa
            var callbackUrl = Url.Action("Enable2fa", "Auth", new { token }, Request.Scheme);

            return Ok("Token has been sent to your email.");
        }

        [HttpPost("confirm-2fa")]

        public async Task<IActionResult> Confirm2fa(string token)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return BadRequest("User not found");

            var result = await _userManager.VerifyTwoFactorTokenAsync(user, "Authenticator", token);

            if (!result)
                return BadRequest("Invalid token");

            user.TwoFactorEnabled = true;

            await _userManager.UpdateAsync(user);

            return Ok("2fa has been enabled successfully.");
        }

        private string GenerateWalletId()
        {
            var random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user); // Fetch roles for the user
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName)
    };

            // Add roles to claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtSettings:ExpirationDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
