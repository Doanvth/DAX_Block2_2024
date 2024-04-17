using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAX_Block2_2024.Entities;

namespace DAX_Block2_2024.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentNewsController : Controller
    {
        private readonly Web_Chia_Se_Tai_LieuContext _context;

        public CommentNewsController(Web_Chia_Se_Tai_LieuContext context)
        {
            _context = context;
        }

        // GET: Admin/CommentNews
        public async Task<IActionResult> Index()
        {
            var web_Chia_Se_Tai_LieuContext = _context.CommentNews.Include(c => c.News).Include(c => c.Users);
            return View(await web_Chia_Se_Tai_LieuContext.ToListAsync());
        }

        // GET: Admin/CommentNews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentNews = await _context.CommentNews
                .Include(c => c.News)
                .Include(c => c.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentNews == null)
            {
                return NotFound();
            }

            return View(commentNews);
        }

        // GET: Admin/CommentNews/Create
        public IActionResult Create()
        {
            ViewData["NewsId"] = new SelectList(_context.News, "Id", "Content");
            ViewData["UsersId"] = new SelectList(_context.Users, "Id", "Address");
            return View();
        }

        // POST: Admin/CommentNews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CommentDate,Content,NewsId,UsersId")] CommentNews commentNews)
        {
            if (ModelState.IsValid)
            {
                _context.Add(commentNews);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NewsId"] = new SelectList(_context.News, "Id", "Content", commentNews.NewsId);
            ViewData["UsersId"] = new SelectList(_context.Users, "Id", "Address", commentNews.UsersId);
            return View(commentNews);
        }

        // GET: Admin/CommentNews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentNews = await _context.CommentNews.FindAsync(id);
            if (commentNews == null)
            {
                return NotFound();
            }
            ViewData["NewsId"] = new SelectList(_context.News, "Id", "Content", commentNews.NewsId);
            ViewData["UsersId"] = new SelectList(_context.Users, "Id", "Address", commentNews.UsersId);
            return View(commentNews);
        }

        // POST: Admin/CommentNews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CommentDate,Content,NewsId,UsersId")] CommentNews commentNews)
        {
            if (id != commentNews.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commentNews);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentNewsExists(commentNews.Id))
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
            ViewData["NewsId"] = new SelectList(_context.News, "Id", "Content", commentNews.NewsId);
            ViewData["UsersId"] = new SelectList(_context.Users, "Id", "Address", commentNews.UsersId);
            return View(commentNews);
        }

        // GET: Admin/CommentNews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentNews = await _context.CommentNews
                .Include(c => c.News)
                .Include(c => c.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentNews == null)
            {
                return NotFound();
            }

            return View(commentNews);
        }

        // POST: Admin/CommentNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commentNews = await _context.CommentNews.FindAsync(id);
            _context.CommentNews.Remove(commentNews);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentNewsExists(int id)
        {
            return _context.CommentNews.Any(e => e.Id == id);
        }
    }
}
