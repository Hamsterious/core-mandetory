using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NIEL.Data;
using NIEL.Models;
using Microsoft.AspNetCore.Authorization;

namespace NIEL.Controllers
{
        [Authorize]
        public class TodoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TodoesController(ApplicationDbContext context)
        {
            _context = context;  
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Todo.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todo
                .SingleOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        [Authorize(Roles = "Organizer,Team Player")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizer,Team Player")]
        public async Task<IActionResult> Create([Bind("Id,Title,Details,Done")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(todo);
        }

        [Authorize(Roles = "Organizer,Team Player,Contributor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todo.SingleOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            return View(todo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizer,Team Player,Contributor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Details,Done")] Todo todo)
        {
            if (id != todo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoExists(todo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(todo);
        }

        [Authorize(Roles = "Team Player,Contributor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todo
                .SingleOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Team Player,Contributor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todo = await _context.Todo.SingleOrDefaultAsync(m => m.Id == id);
            _context.Todo.Remove(todo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TodoExists(int id)
        {
            return _context.Todo.Any(e => e.Id == id);
        }
    }
}
