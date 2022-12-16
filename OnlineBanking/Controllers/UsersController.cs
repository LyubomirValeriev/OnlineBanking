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
    [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
              return View(await _context.users.Include(u => u.role).ToListAsync());
        }

        public IActionResult ChangeRole(int? id)
        {
            if (id == null || _context.users == null)
            {
                return NotFound();
            }

            var user =  _context.users
                .Where(m => m.UserID == id)
                .Include(m => m.role)
                .FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult ChangeRole(string role, int id)
        {
            if (id == null || _context.users == null)
            {
                return NotFound();
            }

            var user = _context.users
                .Where(m => m.UserID == id)
                .Include(m => m.role)
                .FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            var roleObj = _context.roles
                .Where(r => r.role.Equals(role))
                .FirstOrDefault();
            user.role = roleObj;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.users == null)
            {
                return NotFound();
            }

            var user = await _context.users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,UserFirstName,UserLastName,Age,email,UserUsername,password,verificationCode,Active")] User user)
        {
            
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.users == null)
            {
                return NotFound();
            }

            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,UserFirstName,UserLastName,Age,email,UserUsername,password,verificationCode,Active")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.UserID))
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

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.users == null)
            {
                return NotFound();
            }

            var user = await _context.users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.users == null)
            {
                return Problem("Entity set 'AppDbContext.users'  is null.");
            }
            var user = await _context.users.FindAsync(id);
            if (user != null)
            {
                _context.users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return _context.users.Any(e => e.UserID == id);
        }
    }
}
