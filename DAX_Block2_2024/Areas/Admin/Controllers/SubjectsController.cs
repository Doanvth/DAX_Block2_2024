﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAX_Block2_2024.Entities;
using DAX_Block2_2024.Models;

namespace DAX_Block2_2024.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubjectsController : Controller
    {
        private readonly Web_Chia_Se_Tai_LieuContext _context;

        public SubjectsController(Web_Chia_Se_Tai_LieuContext context)
        {
            _context = context;
        }

        // GET: Admin/Subjects
        [Authentication]

        public async Task<IActionResult> Index()
        {
            return View(await _context.Subjects.ToListAsync());
        }

        // GET: Admin/Subjects/Details/5
        [Authentication]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Admin/Subjects/Create
        [Authentication]

        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authentication]

        public async Task<IActionResult> Create([Bind("Id,Name")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Admin/Subjects/Edit/5
        [Authentication]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        // POST: Admin/Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authentication]

        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.Id))
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
            return View(subject);
        }

        // GET: Admin/Subjects/Delete/5
        [Authentication]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Admin/Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authentication]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authentication]

        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }
    }
}
