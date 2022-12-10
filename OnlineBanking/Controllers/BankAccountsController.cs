using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineBanking;
using OnlineBanking.Models;

namespace OnlineBanking.Controllers
{
    public class BankAccountsController : Controller
    {
        private readonly AppDbContext _context;

        public BankAccountsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: BankAccounts
        public async Task<IActionResult> Index()
        {
              return View(await _context.bankAccounts.ToListAsync());
        }

        // GET: BankAccounts/Details/5
        
        public async Task<IActionResult> Details(int? id)
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

        // GET: BankAccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        private bool BankAccountExists(int id)
        {
          return _context.bankAccounts.Any(e => e.ID == id);
        }
    }
}
