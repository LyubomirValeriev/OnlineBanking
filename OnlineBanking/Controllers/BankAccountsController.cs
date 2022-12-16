using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    [Authorize(Policy = "LoggedInPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
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
                .Where(a => a.ID == id)
                .Include(a => a.transactions.OrderBy(tr => tr.date))
                .FirstOrDefaultAsync();

            if (bankAccount == null || (!User.HasClaim("BankId", id.ToString()) && !User.HasClaim("Role", "Admin")) )
            {
                return NotFound();
            }

            return View(bankAccount);
        }

        public IActionResult Deposit(int id)
        {
            return View(id);
        }

        [HttpPost]
        public IActionResult Deposit(double deposit, int id)
        {
            var bankAccount = _context.bankAccounts
                .Where(a => a.ID == id)
                .Include(a => a.transactions)
                .FirstOrDefault();


            var trans = new Transaction
            {
                date = DateTime.UtcNow,
                ToWhom = bankAccount.IBAN,
                from = bankAccount,
                amount = deposit,
                Memo = "Deposit"
            };
            
            bankAccount.transactions.Add(trans);

            bankAccount.Balance += deposit;
            _context.SaveChanges();
                
            return RedirectToAction("Details", int.Parse(User.FindFirstValue("BankId")));
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

        // GET: BankAccounts/Delete/5
        [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.bankAccounts == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.bankAccounts
                .Where(m => m.ID == id)
                .Include(b => b.transactions)
                .FirstOrDefaultAsync();
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

            var recipientBankAccount = _context.bankAccounts
                .Where(b => b.IBAN.Equals(trans.ToWhom))
                .Include(b => b.transactions)
                .FirstOrDefault();

            trans.amount = Math.Abs(trans.amount);

            if (trans.amount > bankAccount.Balance || recipientBankAccount == null)
            {
                return Redirect("Error");
            }


            trans.date = DateTime.UtcNow;
            trans.from = bankAccount;

            

            var recipientTrans = new Transaction
            {
                amount = trans.amount,
                from = bankAccount,
                Memo= trans.Memo,
                date = trans.date,
                ToWhom = recipientBankAccount.IBAN
            };
            

            bankAccount.Balance -= trans.amount;
            recipientBankAccount.Balance += trans.amount;

            trans.amount *= -1;
            if(bankAccount.transactions == null) { bankAccount.transactions = new List<Transaction>(); }
            bankAccount.transactions.Add(trans);
            recipientBankAccount.transactions.Add(recipientTrans);

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", int.Parse(User.FindFirstValue("BankId")));
        }
        public IActionResult MakeDeposit ()
        {
            return View();
        }
        
        private bool BankAccountExists(int id)
        {
          return _context.bankAccounts.Any(e => e.ID == id);
        }
    }
}
