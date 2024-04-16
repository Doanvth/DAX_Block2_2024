using DAX_Block2_2024.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAX_Block2_2024.Controllers
{
    public class NewDetailController : Controller
    {
        private Web_Chia_Se_Tai_LieuContext _context;
        public class ViewModelNewDetails
        {
            public IList<News> RelatedNewsTake4 { get; set; }
            public IList<News> RelatedNewsTakeAll { get; set; }
            public IList<CommentNews> CommentOfNews { get; set; }
            public News NewsDetails { get; set; }
        }
        // GET: NewDetailController/Details/5
        //Bỏ này vào bên HomeUser 
        //asp-controller="NewDetail" asp-action="Details" asp-route-id="@item.Id"
        [HttpGet]
        [Route("New/Detail/{id}")]
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var news = await _context.News
                .Include(d => d.CreateByNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            var relatedNewsTake4 = await _context.News
                .Include(d => d.CreateByNavigation)
                .Where(x => x.SubjectsNews == news.SubjectsNews)
                .OrderBy(x => x.CreateDate)
                .Take(4).ToListAsync();

            var relatedNewsTakeAll = await _context.News
                .Include(d => d.CreateByNavigation)
                .Where(x => x.SubjectsNews == news.SubjectsNews)
                .OrderBy(x => x.CreateDate)
                .Skip(4)
                .ToListAsync();

            var commentsOfNews = await _context.CommentNews
                .Include(d => d.Users)
                .Where(x => x.NewsId == id)
                .OrderBy(x => x.CommentDate)
                .ToListAsync();

            if (news == null)
            {
                return NotFound();
            }
            ViewModelNewDetails viewmodel = new ViewModelNewDetails()
            {
                RelatedNewsTake4 = relatedNewsTake4,
                RelatedNewsTakeAll = relatedNewsTakeAll,
                CommentOfNews = commentsOfNews,
                NewsDetails = news
            };
            return View(news);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostComment(string comment, string username, int newsId, DateTime commentDate)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == username);
                if (user == null)
                {
                    return Json(new { success = false});
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
