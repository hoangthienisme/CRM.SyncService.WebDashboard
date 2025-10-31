using System.Security.Cryptography;
using System.Text;
using CRM.SyncService.WebDashboard.Data;
using CRM.SyncService.WebDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.SyncService.WebDashboard.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        public AccountController(AppDbContext db) => _db = db;

        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var hash = HashPassword(password);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == hash);

            if (user == null)
            {
                ViewBag.Error = "Sai email hoặc mật khẩu!";
                return View();
            }

            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserEmail", user.Email);

            return RedirectToAction("Index", "Dashboard");
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
