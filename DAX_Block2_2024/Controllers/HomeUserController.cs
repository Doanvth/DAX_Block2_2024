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
    public class ViewModel
    {
        public IList<Document> Document { get; set; }
        public IList<Document> Document8 { get; set; }
        public IList<News> News3 { get; set; }
        public IList<News> News { get; set; }
        public IList<News> NewsFull { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public IList<Category> Category3 { get; set; }
    }
    public class ViewModelPaging<T>
    {
        public List<T> Items { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }

    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
    }
    public class HomeUserController : Controller
    {
        private readonly Web_Chia_Se_Tai_LieuContext _context;

        public HomeUserController(Web_Chia_Se_Tai_LieuContext context)
        {
            _context = context;
        }

        // GET: HomeUser
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 2;

            var web_Chia_Se_Tai_LieuContext = await _context.Documents
                .Include(d => d.PostByNavigation)
                .Include(d => d.DocumentsCategories)
                .OrderByDescending(d => d.DatePost)
                .Take(5)
                .ToListAsync();

            var web_Chia_Se_Tai_LieuContextDoc8 = await _context.Documents
               .Include(d => d.PostByNavigation)
               .Include(d => d.DocumentsCategories)
               .OrderByDescending(d => d.DatePost)
               .Take(8)
               .ToListAsync();

            var web_Chia_Se_Tai_LieuContext2 = await _context.News
                .Include(d => d.CreateByNavigation)
                .Take(1)
                .ToListAsync();

            var web_Chia_Se_Tai_LieuContext3 = await _context.News
                .Include(d => d.CreateByNavigation)
                .Take(3)
                .ToListAsync();

            var newFull = await _context.News
                .Include(d => d.CreateByNavigation)
                .ToListAsync();

            var categoryList = await _context.Categories
              .OrderByDescending(d => d.Id)
              .Take(3)
              .ToListAsync();

            var paginatedDocuments = newFull.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new ViewModel
            {
                Document = web_Chia_Se_Tai_LieuContext,
                Document8 = web_Chia_Se_Tai_LieuContextDoc8,
                News = web_Chia_Se_Tai_LieuContext2,
                News3 = web_Chia_Se_Tai_LieuContext3,
                NewsFull = paginatedDocuments,
                Category3 = categoryList,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = newFull.Count
                }
            };
            return View(viewModel);

        }

        // GET: HomeUser/Details/5
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
    }
}
