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
        public NewDetailController(Web_Chia_Se_Tai_LieuContext context)
        {
            _context = context;
        }


        public class ViewModelNewDetails
        {
            public IList<News> RelatedNewsTake4 { get; set; }
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

            // Fetch the main news item including its subjects
            var news = await _context.News
                .Include(n => n.CreateByNavigation)
                .Include(n => n.SubjectsNews)
                .ThenInclude(s => s.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (news == null)
            {
                return NotFound("News item not found.");
            }

            // Extract subject IDs associated with this news item
            var subjectIds = news.SubjectsNews.Select(sn => sn.SubjectId).ToList();

            // Fetch up to 4 related news items that share any of the same subject IDs
            var relatedNewsTake4 = await _context.News
                .Include(n => n.CreateByNavigation)
                .Where(n => n.SubjectsNews.Any(sn => subjectIds.Contains(sn.SubjectId) && sn.NewsId != news.Id))
                .OrderBy(n => n.CreateDate)
                .Take(4)
                .ToListAsync();

            // Fetch comments associated with the news item
            var commentsOfNews = await _context.CommentNews
                .Include(c => c.Users)
                .Where(c => c.NewsId == id)
                .OrderBy(c => c.CommentDate)
                .ToListAsync();

            ViewModelNewDetails viewModel = new ViewModelNewDetails
            {
                RelatedNewsTake4 = relatedNewsTake4,
                CommentOfNews = commentsOfNews,
                NewsDetails = news
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetMoreRelatedNews(int newsId)
        {
            var news = await _context.News
                .Include(n => n.CreateByNavigation)
                .Include(n => n.SubjectsNews)
                .ThenInclude(sn => sn.Subject)
                .FirstOrDefaultAsync(m => m.Id == newsId);

            if (news == null)
            {
                return NotFound("News not found");
            }

            var subjectIds = news.SubjectsNews.Select(sn => sn.SubjectId).ToList();

            var additionalNews = await _context.News
                .Include(n => n.CreateByNavigation)
                .Include(n => n.SubjectsNews)
                .Where(n => n.Id != newsId && n.SubjectsNews.Any(sn => subjectIds.Contains(sn.SubjectId)))
                .ToListAsync();

            return PartialView("_RelatedNews", additionalNews);
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
