#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Data;
using MyPortfolio.Models;

namespace MyPortfolio.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _host;

        public ProjectController(ApplicationDbContext context, IWebHostEnvironment host)
        {
            _context = context;
            this._host = host;
        }

        // GET: Project
        [Route("/Admin/Projects")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Project.Include(p => p.Category).OrderByDescending(item => item.Created);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Project/Details/5
        [Route("/Admin/Projects/Details/{id?}")]
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Project/Create
        [Route("/Admin/Projects/Create")]
        [Authorize]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id");
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Admin/Projects/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,Context,Created,Url,Slug,ImageFile,ImageTitle,CategoryId")] Project project)
        {
            if (ModelState.IsValid)
            {
                //Define wwwrootpath, filename, extension
                string wwwRootPath = _host.WebRootPath;
                string file = Path.GetFileNameWithoutExtension(project.ImageFile.FileName);
                string extension = Path.GetExtension(project.ImageFile.FileName);
                //Make file name "unique"
                file = file + DateTime.Now.ToString("yyyyMMddssff") + extension;

                //Otherwise ImageName will be null in database
                project.ImageName = file;

                string ImagePath = Path.Combine(wwwRootPath, "Images", file);
                //Upload to defined path with FileStream
                using (var fileStream = new FileStream(ImagePath, FileMode.Create))
                {
                    await project.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", project.CategoryId);
            return View(project);
        }

        // GET: Project/Edit/5
        [Route("/Admin/Projects/Edit/{id?}")]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", project.CategoryId);
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Admin/Projects/Edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Context,Created,Url,Slug,ImageFile,ImageTitle,CategoryId")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Define wwwrootpath, filename, extension
                    string wwwRootPath = _host.WebRootPath;
                    string file = Path.GetFileNameWithoutExtension(project.ImageFile.FileName);
                    string extension = Path.GetExtension(project.ImageFile.FileName);
                    //Make file name "unique"
                    file = file + DateTime.Now.ToString("yyyyMMddssff") + extension;

                    //Otherwise ImageName will be null in database
                    project.ImageName = file;

                    string ImagePath = Path.Combine(wwwRootPath, "Images", file);
                    //Upload to defined path with FileStream
                    using (var fileStream = new FileStream(ImagePath, FileMode.Create))
                    {
                        await project.ImageFile.CopyToAsync(fileStream);
                    }

                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", project.CategoryId);
            return View(project);
        }

        // GET: Project/Delete/5
        [Route("/Admin/Projects/Delete/{id?}")]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var project = await _context.Project
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(project);
        }

        // POST: Project/Delete/5
        [Route("/Admin/Projects/Delete/{id?}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);

            //Delete the Image from folder
            var imagePath = Path.Combine(_host.WebRootPath, "Images", project.ImageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            //Delete in database
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.Id == id);
        }
    }
}
