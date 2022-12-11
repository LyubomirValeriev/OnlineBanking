using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineBanking;
using OnlineBanking.Models;

namespace OnlineBanking.Controllers
{
    [Authorize(Policy = "UserPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class BankAccountsController : Controller
    {
        private readonly AppDbContext _context;

        public BankAccountsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: BankAccounts
        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Index()
        {
              return View(await _context.bankAccounts.ToListAsync());
        }

        // GET: BankAccounts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (_context.bankAccounts == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.bankAccounts
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bankAccount == null || (!User.HasClaim("BankId", id.ToString()) && !User.HasClaim("Role", "Admin")) )
            {
                return NotFound();
            }

            return View(bankAccount);
        }

        // GET: BankAccounts/Create
        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([Bind("ID,IBAN,Balance,Holder")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bankAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Edit/5
        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.bankAccounts == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.bankAccounts.FindAsync(id);
            if (bankAccount == null)
            {
                return NotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Edit(int id, [Bind("ID,IBAN,Balance,Holder")] BankAccount bankAccount)
        {
            if (id != bankAccount.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bankAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankAccountExists(bankAccount.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Delete/5
        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.bankAccounts == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.bankAccounts
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.bankAccounts == null)
            {
                return Problem("Entity set 'AppDbContext.BankAccount'  is null.");
            }
            var bankAccount = await _context.bankAccounts.FindAsync(id);
            if (bankAccount != null)
            {
                _context.bankAccounts.Remove(bankAccount);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult MakeTransaction()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MakeTransaction(Transaction trans)
        {
            var bankAccountClaim = User.Claims.Where(cl => cl.Type.Equals("BankId")).FirstOrDefault();
            var bankAccountId = int.Parse(bankAccountClaim.Value);
            var bankAccount = _context.bankAccounts
                .Where(b => b.ID == bankAccountId)
                .Include(b => b.transactions)
                .FirstOrDefault();

            var recipientBankAccount = _context.bankAccounts.Where(b => b.IBAN.Equals(trans.ToWhom)).FirstOrDefault();

            if (trans.amount > bankAccount.Balance || recipientBankAccount == null)
            {
                return Redirect("Error");
            }

            trans.date = DateTime.UtcNow;
            trans.from = bankAccount;

            bankAccount.Balance -= trans.amount;
            recipientBankAccount.Balance += trans.amount;

            if(bankAccount.transactions == null) { bankAccount.transactions = new List<Transaction>(); }
            bankAccount.transactions.Add(trans);

            await _context.SaveChangesAsync();
            return View();
        }

        private bool BankAccountExists(int id)
        {
          return _context.bankAccounts.Any(e => e.ID == id);
        }
    }
}
