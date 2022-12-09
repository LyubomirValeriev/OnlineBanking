using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Common;
using OnlineBanking.Models;
using System;
using System.Diagnostics;

namespace OnlineBanking.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext context;

        public HomeController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
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

        public IActionResult RegisterUser(User user)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int ibanLen = 20;

            user.role = context.roles
                .Where(role => role.role.Equals("User"))
                .FirstOrDefault();
            user.password = BCrypt.Net.BCrypt.HashPassword(user.password);

            context.users.Add(user);
            context.SaveChanges(); // generates unique id


            var random = new Random(user.UserID);

            var iban = new string(Enumerable.Repeat(chars, ibanLen)
            .Select(s => s[random.Next(s.Length)]).ToArray()); 
            

            user.bankAccount = new BankAccount
            {
                IBAN = $"BG{iban}",
                Balance = 0,
                Holder = $"{user.UserFirstName} {user.UserLastName}"
            };

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