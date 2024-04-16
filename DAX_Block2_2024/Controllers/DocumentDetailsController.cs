using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAX_Block2_2024.Entities;
using DAX_Block2_2024.Models;

namespace DAX_Block2_2024.Controllers
{
    public class DocumentDetailsController : Controller
    {
        public class ViewModelDocDetails
        {
            public IList<Document> Document { get; set; }
            public IList<Document> Document3 { get; set; }
            public Document DocumentDetails { get; set; }
        }

        private readonly Web_Chia_Se_Tai_LieuContext _context;

        public DocumentDetailsController(Web_Chia_Se_Tai_LieuContext context)
        {
            _context = context;
        }

        // GET: DocumentDetails
        [Authentication]

        public async Task<IActionResult> Index(int? id)
        {
            var web_Chia_Se_Tai_LieuContext = await _context.Documents.Include(d => d.PostByNavigation).Take(1).ToListAsync();
            var web_Chia_Se_Tai_LieuContext2 = await _context.Documents.Include(d => d.PostByNavigation).Take(3).ToListAsync();

            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.PostByNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (document == null)
            {
                return NotFound();
            }

            var viewModel = new ViewModelDocDetails
            {
                Document = web_Chia_Se_Tai_LieuContext,
                Document3 = web_Chia_Se_Tai_LieuContext2,
                DocumentDetails = document
            };
            return View(viewModel);
        }

        // GET: DocumentDetails/Details/5
        [Authentication]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.PostByNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authentication]

        public async Task<IActionResult> PostComment(string comment, string username, int newsId, DateTime commentDate)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == username);
                if (user == null)
                {
                    return Json(new { success = false });
                }

                var newComment = new CommentNews
                {
                    UsersId = user.Id,
                    NewsId = newsId,
                    Content = comment,
                    CommentDate = commentDate
                };

                _context.CommentNews.Add(newComment);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
