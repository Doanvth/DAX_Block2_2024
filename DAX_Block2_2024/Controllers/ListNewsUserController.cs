using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAX_Block2_2024.Entities;

namespace DAX_Block2_2024.Controllers
{
    public class ListNewsUserController : Controller
    {
        private readonly Web_Chia_Se_Tai_LieuContext _context;

        public ListNewsUserController(Web_Chia_Se_Tai_LieuContext context)
        {
            _context = context;
        }

        // GET: ListNewsUser

        public async Task<IActionResult> Index()
        {
            var web_Chia_Se_Tai_LieuContext = _context.News.Include(n => n.CreateByNavigation);
            return View(await web_Chia_Se_Tai_LieuContext.ToListAsync());
        }

        // GET: ListNewsUser/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.CreateByNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: ListNewsUser/Create
        public IActionResult Create()
        {
            ViewData["CreateBy"] = new SelectList(_context.Users, "Id", "Address");
            return View();
        }

        // POST: ListNewsUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Content,Image,CreateDate,CreateBy,Status")] News news)
        {
            if (ModelState.IsValid)
            {
                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreateBy"] = new SelectList(_context.Users, "Id", "Address", news.CreateBy);
            return View(news);
        }

        // GET: ListNewsUser/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["CreateBy"] = new SelectList(_context.Users, "Id", "Address", news.CreateBy);
            return View(news);
        }

        // POST: ListNewsUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Content,Image,CreateDate,CreateBy,Status")] News news)
        {
            if (id != news.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.Id))
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
            ViewData["CreateBy"] = new SelectList(_context.Users, "Id", "Address", news.CreateBy);
            return View(news);
        }

        // GET: ListNewsUser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.CreateByNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: ListNewsUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }
    }
}
