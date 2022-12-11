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
using System.Security.Claims;

namespace OnlineBanking.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext context;
        private readonly RoleManager<Role> _roleManager;
        public HomeController(AppDbContext context )
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
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
            user = retrievedUser;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserUsername),
                new Claim(ClaimTypes.Email, user.email),
                new Claim("Role", user.role.role),
                new Claim("BankId", user.bankAccount.ID.ToString()),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        


            return Redirect($"/BankAccounts/Details/{user.bankAccount.ID}"); // redirect to user account
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



           
            

            user.bankAccount = new BankAccount
            {
                IBAN = "BG",
                Balance = 0,
                Holder = $"{user.UserFirstName} {user.UserLastName}"
            };

            context.users.Add(user);
            context.SaveChanges();

            var random = new Random(user.UserID);

            var iban = new string(Enumerable.Repeat(chars, ibanLen)
            .Select(s => s[random.Next(s.Length)]).ToArray());

            user.bankAccount.IBAN = $"BG{iban}";

            context.bankAccounts.Update(user.bankAccount);
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