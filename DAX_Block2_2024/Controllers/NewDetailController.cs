using DAX_Block2_2024.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DAX_Block2_2024.Controllers
{
    public class NewDetailController : Controller
    {
        private Web_Chia_Se_Tai_LieuContext _context;
        private readonly ILogger<NewDetailController> _logger;
        public NewDetailController(ILogger<NewDetailController> logger, Web_Chia_Se_Tai_LieuContext context)
        {
            _context = context;
            _logger = logger;
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


        public class CommentDTO
        {
            public string Comment { get; set; }
            public string Username { get; set; }
            public int NewsId { get; set; }
            public string CommentDate { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> PostComment([FromBody] CommentDTO commentData)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == commentData.Username);
                if (user == null)
                {
                    return Json(new { success = false, message = "User not found." });
                }

                if (!DateTime.TryParseExact(commentData.CommentDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    return Json(new { success = false, message = "Invalid date format." });
                }

                var newComment = new CommentNews
                {
                    UsersId = user.Id,
                    NewsId = commentData.NewsId,
                    Content = commentData.Comment,
                    CommentDate = parsedDate
                };

                _context.CommentNews.Add(newComment);
                await _context.SaveChangesAsync();
                var formattedDate = ((DateTime)newComment.CommentDate).ToString("MM/dd/yyyy");
                return Json(new
                {
                    success = true,
                    fullName = user.FullName,
                    commentDate = formattedDate,
                    comment = newComment.Content
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while posting the comment.");
                _logger.LogError(ex, "An error occurred while posting the comment.");
                return Json(new { success = false, message = "An error occurred while posting the comment." });
            }
        }
            }
}