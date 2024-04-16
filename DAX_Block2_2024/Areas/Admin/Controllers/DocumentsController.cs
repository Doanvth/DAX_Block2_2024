using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAX_Block2_2024.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using DAX_Block2_2024.Models;

namespace DAX_Block2_2024.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DocumentsController : Controller
    {
        private readonly Web_Chia_Se_Tai_LieuContext _context;
        private readonly ILogger<Web_Chia_Se_Tai_LieuContext> _logger;
        private readonly IWebHostEnvironment _web;
        private readonly IHostingEnvironment _environment;
        private IHostingEnvironment Environment;
        public List<Document> Documents { get; set; }


        public DocumentsController(Web_Chia_Se_Tai_LieuContext context, ILogger<Web_Chia_Se_Tai_LieuContext> logger, IWebHostEnvironment env, IHostingEnvironment environment)
        {
            _context = context;
            _web = env;
            _logger = logger;
            _environment = environment;
        }

        // GET: Admin/Documents
        [Authentication]

        public async Task<IActionResult> Index()
        {
            var documents = await _context.Documents.Include(d => d.PostByNavigation).ToListAsync();
            return View(documents);
        }
        // GET: Admin/Documents/Details/5
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

        // GET: Admin/Documents/Create
        //[HttpGet("CreateDoc")]
        [Authentication]

        public IActionResult Create()
        {
            ViewData["PostBy"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["TagsId"] = new SelectList(_context.Tags, "Id", "Name");
            ViewData["CategoriesId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Admin/Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authentication]

        public async Task<IActionResult> Create([Bind("Id,Name,Description,Content,DatePost,PostBy,Image,FilePath,Rate,Files,Status,Price,TagsId,CategoriesId")] Document document, IFormFile postedFile, IFormFile postImg)
        {
            if (ModelState.IsValid)
            {
                string wwwPath = _web.WebRootPath;
                string pathFile = Path.Combine(wwwPath, "UploadDocuments");
                string pathImg = Path.Combine(wwwPath, "UploadDocuments");
                if (!Directory.Exists(pathImg))
                {
                    Directory.CreateDirectory(pathImg);
                }
                if (!Directory.Exists(pathFile))
                {
                    Directory.CreateDirectory(pathFile);
                }
                if (!Directory.Exists(pathImg) && !Directory.Exists(pathFile))
                {
                    Directory.CreateDirectory(pathImg);
                    Directory.CreateDirectory(pathFile);
                }
                if (postedFile != null &&
                    postedFile.Length > 0 &&
                    postImg != null &&
                    postImg.Length > 0)
                {
                    string fileName = Path.GetFileName(postedFile.FileName);
                    string targetPath = Path.Combine(pathFile, fileName);

                    string imgName = Path.GetFileName(postImg.FileName);
                    string targetPathImg = Path.Combine(pathImg, imgName);

                    if (System.IO.File.Exists(targetPath))
                    {
                        ModelState.AddModelError(string.Empty, "Tên tệp đã tồn tại trong thư mục Uploads.");
                        ViewData["PostBy"] = new SelectList(_context.Users, "Id", "FullName", document.PostBy);
                        ViewData["TagsId"] = new SelectList(_context.Tags, "Id", "Name", document.TagsId);
                        ViewData["CategoriesId"] = new SelectList(_context.Categories, "Id", "Name", document.CategoriesId);
                        return View(document);
                    }
                    using (var stream = new FileStream(targetPath, FileMode.Create))
                    {
                        await postedFile.CopyToAsync(stream);
                    }

                    using (var imgStream = new FileStream(targetPathImg, FileMode.Create))
                    {
                        await postImg.CopyToAsync(imgStream);
                    }


                    // Lưu đường dẫn tệp vào đối tượng sản phẩm
                    document.Image = "/UploadDocuments/" + imgName;
                    document.FilePath = "/UploadDocuments/" + fileName;
                }
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PostBy"] = new SelectList(_context.Users, "Id", "FullName", document.PostBy);
            ViewData["TagsId"] = new SelectList(_context.Tags, "Id", "Name", document.TagsId);
            ViewData["CategoriesId"] = new SelectList(_context.Categories, "Id", "Name", document.CategoriesId);
            return View(document);
        }
        // GET: Admin/Documents/Edit/5
        [Authentication]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            ViewData["PostBy"] = new SelectList(_context.Users, "Id", "FullName", document.PostBy);
            ViewData["TagsId"] = new SelectList(_context.Tags, "Id", "Name", document.TagsId);
            ViewData["CategoriesId"] = new SelectList(_context.Categories, "Id", "Name", document.CategoriesId);
            return View(document);
        }

        // POST: Admin/Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authentication]

        public async Task<IActionResult> EditDoc(int id, [Bind("Id,Name,Description,Content,DatePost,PostBy,Image,FilePath,Rate,Files,Status,Price,TagsId,CategoriesId")] Document document, IFormFile postedFile, IFormFile postImg)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string wwwPath = _web.WebRootPath;
                string pathFile = Path.Combine(wwwPath, "UploadDocuments");
                string pathImg = Path.Combine(wwwPath, "UploadDocuments");

                if (!Directory.Exists(pathImg))
                {
                    Directory.CreateDirectory(pathImg);
                }
                if (!Directory.Exists(pathFile))
                {
                    Directory.CreateDirectory(pathFile);
                }
                try
                {
                    if (postedFile != null &&
                   postedFile.Length > 0 &&
                   postImg != null &&
                   postImg.Length > 0)
                    {
                        string fileName = Path.GetFileName(postedFile.FileName);
                        string targetPath = Path.Combine(pathFile, fileName);

                        string imgName = Path.GetFileName(postImg.FileName);
                        string targetPathImg = Path.Combine(pathImg, imgName);

                        if (System.IO.File.Exists(targetPath))
                        {
                            ModelState.AddModelError(string.Empty, "Tên tệp đã tồn tại trong thư mục Uploads.");
                            ViewData["PostBy"] = new SelectList(_context.Users, "Id", "FullName", document.PostBy);
                            return View(document);
                        }

                        using (var stream = new FileStream(targetPath, FileMode.Create))
                        {
                            await postedFile.CopyToAsync(stream);
                        }

                        using (var imgStream = new FileStream(targetPathImg, FileMode.Create))
                        {
                            await postImg.CopyToAsync(imgStream);
                        }

                        // Lưu đường dẫn tệp vào đối tượng sản phẩm
                        document.Image = "/UploadDocuments/" + imgName;
                        document.FilePath = "/UploadDocuments/" + fileName;
                    }

                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
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
            ViewData["PostBy"] = new SelectList(_context.Users, "Id", "FullName", document.PostBy);
            ViewData["TagsId"] = new SelectList(_context.Tags, "Id", "Name", document.TagsId);
            ViewData["CategoriesId"] = new SelectList(_context.Categories, "Id", "Name", document.CategoriesId);
            return View(document);
        }

        // GET: Admin/Documents/Delete/5
        [Authentication]

        public async Task<IActionResult> Delete(int? id)
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

        // POST: Admin/Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authentication]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authentication]

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }

        [Authentication]

        public void OnGet()
        {
            string[] filePaths = Directory.GetFiles(Path.Combine(this._environment.WebRootPath, "UploadDocuments/"));
            this.Documents = new List<Document>();
            foreach (string file in filePaths)
            {
                this.Documents.Add(new Document { FilePath = Path.GetFileName(file) });
            }
        }

        [Authentication]

        public FileResult OnGetDownloadFile(string filePath)
        {
            string path = Path.Combine(this._environment.WebRootPath, "UploadDocuments/") + filePath;
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", filePath);
        }
    }
}
