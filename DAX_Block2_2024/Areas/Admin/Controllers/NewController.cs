using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAX_Block2_2024.Entities;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using DAX_Block2_2024.Models;

namespace DAX_Block2_2024.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewController : Controller
    {
        private readonly Web_Chia_Se_Tai_LieuContext _context;

        public NewController(Web_Chia_Se_Tai_LieuContext context)
        {
            _context = context;
        }

        // GET: Admin/New
        [Authentication]

        public async Task<IActionResult> Index()
        {
            var web_Chia_Se_Tai_LieuContext = _context.News.Include(n => n.CreateByNavigation);
            return View(await web_Chia_Se_Tai_LieuContext.ToListAsync());
        }

        // GET: Admin/New/Details/5
        [Authentication]

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

        // GET: Admin/New/Create
        [Authentication]

        public IActionResult Create()
        {
            ViewData["CreateBy"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name"); // Ensure this line is added
            return View();
        }

        // POST: Admin/New/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authentication]

        public async Task<IActionResult> Create([Bind("Id,Title,Description,Content,Image,Status,CreateDate,CreateBy")] News news, IFormFile ImageFile, int? subjectId)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = WebUtility.HtmlEncode(Path.GetFileName(ImageFile.FileName));
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/newsImage", fileName);

                    if (!System.IO.File.Exists(filePath))
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(fileStream);
                        }
                    }
                    news.Image = fileName;
                }
                _context.Add(news);
                await _context.SaveChangesAsync();
                if (subjectId.HasValue)
                {
                    var subjectNews = new SubjectsNews
                    {
                        NewsId = news.Id,
                        SubjectId = subjectId.Value
                    };
                    _context.SubjectsNews.Add(subjectNews);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreateBy"] = new SelectList(_context.Users, "Id", "UserName", news.CreateBy);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
            return View(news);
        }

        // GET: Admin/New/Edit/5
        [Authentication]

        public IActionResult Edit()
        {
            ViewData["CreateBy"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["NewsList"] = new SelectList(_context.News, "Id", "Content");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
            return View();
        }
        [HttpGet]
        [Authentication]

        public IActionResult GetNewsDetails(int id)
        {
            var news = _context.News
                .Include(n => n.SubjectsNews) 
                .SingleOrDefault(n => n.Id == id);

            if (news == null)
            {
                return NotFound();
            }

            string formattedDate = news.CreateDate.ToString("dd/MM/yyyy");
            var firstSubjectId = news.SubjectsNews.FirstOrDefault()?.SubjectId ?? -1;

            return Ok(new
            {
                title = news.Title,
                content = news.Content,
                description = news.Description,
                createDate = formattedDate,
                image = news.Image,
                createdBy = news.CreateBy,
                status = news.Status,
                subjectId = firstSubjectId 
            });
        }



        // POST: Admin/New/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authentication]

        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Content,Image,Status,CreateDate,CreateBy")] News news, IFormFile ImageFile, int? subjectId)
        {
            if (id != news.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        var fileName = WebUtility.HtmlEncode(Path.GetFileName(ImageFile.FileName));
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/newsImage", fileName);

                        if (!System.IO.File.Exists(filePath))
                        {
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await ImageFile.CopyToAsync(fileStream);
                            }
                        }
                        news.Image = fileName;
                    }
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                    if (subjectId.HasValue)
                    {
                        var subjectNews = new SubjectsNews
                        {
                            NewsId = news.Id,
                            SubjectId = subjectId.Value
                        };
                        _context.SubjectsNews.Update(subjectNews);
                        await _context.SaveChangesAsync();
                    }
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
            ViewData["NewsList"] = new SelectList(_context.News, "Id", "Content", news.Id);
            ViewData["CreateBy"] = new SelectList(_context.Users, "Id", "UserName", news.CreateBy);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
            return View(news);
        }


        //GET: Admin/New/Delete/5
        [Authentication]

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

        // POST: Admin/New/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authentication]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authentication]


        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }
    }


}
