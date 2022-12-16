using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using OnlineBanking.Models;
using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Buffers.Text;
using Microsoft.AspNetCore.Authorization;

namespace OnlineBanking.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly AppDbContext context;
        private readonly RoleManager<Role> _roleManager;
        private readonly EmailService _emailService;
        public HomeController(AppDbContext context )
        {
            this.context = context;
            this._emailService = new EmailService();
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult SendMessage()
        {
            _emailService.SendEmail("stef4oben88@gmail.com", "Test message", "this is a test message");
            return Content("Mail sent");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginPost(User user)
        {

            //if (!ModelState.IsValid) return RedirectToAction("Login");
            //user.password = BCrypt.Net.BCrypt.HashPassword(user.password);

            User? retrievedUser = context.users
                .Where(u => u.UserUsername.Equals(user.UserUsername))
                .Include(u => u.bankAccount)
                .Include(u => u.role)
                .FirstOrDefault();

            if (retrievedUser == null || !BCrypt.Net.BCrypt.Verify(user.password, retrievedUser.password))
            {
                return RedirectToAction("Login");
            }

            if (!retrievedUser.Active)
            {
                return Content("Your account is not yet verified!");
            }

            user = retrievedUser;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserUsername),
                new Claim(ClaimTypes.Email, user.email),
                new Claim("Role", user.role.role),
            };

            if(user.bankAccount!= null) { claims.Add(new Claim("BankId", user.bankAccount.ID.ToString())); }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);



            if (user.bankAccount != null)
                return Redirect($"/BankAccounts/Details/{user.bankAccount.ID}"); // redirect to user account
            else
                return Redirect($"/BankAccounts");
        }   

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Verify(string code)
        {
            var account = context.users.Where(u => u.verificationCode.Equals(code)).FirstOrDefault();
            if (account == null) return Error();

            account.Active = true;

            context.SaveChanges();
            return Content("Your account is verified! You may login now.");
        }

        public IActionResult RegisterUser(User user)
        {
            //if (!ModelState.IsValid) return RedirectToAction("Index");

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int ibanLen = 20;

            Role? retrivedRole = context.roles
                .Where(role => role.role.Equals("User"))
                .FirstOrDefault();
            if (retrivedRole == null)
            {
                return RedirectToAction("Register"); // Internal Server error
            }
            user.role = retrivedRole;
            user.password = BCrypt.Net.BCrypt.HashPassword(user.password);
            user.Active = false;
            user.verificationCode = "";

          

            user.bankAccount = new BankAccount
            {
                IBAN = "BG",
                Balance = 0,
                Holder = $"{user.UserFirstName} {user.UserLastName}"
            };
            user.verificationCode = " ";
            context.users.Add(user);
            context.SaveChanges();

            var random = new Random(user.UserID);

            var iban = new string(Enumerable.Repeat(chars, ibanLen)
            .Select(s => s[random.Next(s.Length)]).ToArray());

            user.bankAccount.IBAN = $"BG{iban}";
            user.verificationCode = Base64UrlTextEncoder.Encode(System.Text.Encoding.UTF8.GetBytes(BCrypt.Net.BCrypt.HashPassword(user.UserID.ToString())));
            _emailService.SendEmail(user.email, "Verification for bank account", $"Click this to verify account: <a href=https://localhost:7094/Home/Verify?code={user.verificationCode}>");

            context.bankAccounts.Update(user.bankAccount);
            context.users.Update(user);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}