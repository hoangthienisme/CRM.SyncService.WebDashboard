using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CRM.SyncService.WebDashboard.Data;
using CRM.SyncService.WebDashboard.Models;
using CRM.SyncService.WebDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CRM.SyncService.WebDashboard.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        private readonly TelegramAlertService _telegram;
        private readonly IConfiguration _config;
        public AccountController(AppDbContext db, TelegramAlertService telegram, IConfiguration config)
         { 
            _db = db;
            _telegram = telegram;
            _config = config;
        }

        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var hash = HashPassword(password);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == hash);
            //if (email != "admin@gmail.com" || password != "123")
            //{
            //    await _telegram.SendAlertAsync($"Login failed: user={email}, IP={HttpContext.Connection.RemoteIpAddress}");
            //    return Unauthorized();
            //}
            if (user == null)
            {
                ViewBag.Error = "Sai email hoặc mật khẩu!";
                return View();
            }

            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserEmail", user.Email);

            return RedirectToAction("Index", "Dashboard");
        }
        [HttpPost("/api/login")]
        public IActionResult ApiLogin([FromBody] LoginDto model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return BadRequest("Missing credentials");

            var hashed = HashPassword(model.Password);
            var user = _db.Users.FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == hashed);

            if (user == null)
                return Unauthorized();

            // Tạo JWT
            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim("FullName", user.FullName)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }


        public class LoginDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model, string passwordConfirm)
        {
            if (await _db.Users.AnyAsync(u => u.Email == model.Email))
            {
                ViewBag.Error = "Email đã tồn tại!";
                return View(model);
            }

            if (model.PasswordHash != passwordConfirm)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp!";
                return View(model);
            }

            model.PasswordHash = HashPassword(model.PasswordHash);
            _db.Add(model);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Đăng ký thành công, vui lòng đăng nhập!";
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
