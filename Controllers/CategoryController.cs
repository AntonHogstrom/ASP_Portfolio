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

namespace MyPortfolio.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _host;

        public CategoryController(ApplicationDbContext context, IWebHostEnvironment host)
        {
            _context = context;
            this._host = host;
        }

        // GET: Category
        [Route("/Admin/Category")]
        [Route("/Admin/Categories")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.OrderByDescending(item => item.Id).ToListAsync());
        }

        // GET: Category/Details/5
        [Route("/Admin/Categories/Details/{id?}")]
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        [Route("/Admin/Categories/Create")]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Admin/Categories/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title,Color,ImageFile,ImageTitle")] Category category)
        {
            if (ModelState.IsValid)
            {
                //Define wwwrootpath, filename, extension
                string wwwRootPath = _host.WebRootPath;
                string file = Path.GetFileNameWithoutExtension(category.ImageFile.FileName);
                string extension = Path.GetExtension(category.ImageFile.FileName);
                //Make file name "unique"
                file = file + DateTime.Now.ToString("yyyyMMddssff") + extension;

                //Otherwise ImageName will be null in database
                category.ImageName = file;

                string ImagePath = Path.Combine(wwwRootPath, "Images", file);
                //Upload to defined path with FileStream
                using (var fileStream = new FileStream(ImagePath, FileMode.Create))
                {
                    await category.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Edit/5
        [Route("/Admin/Categories/Edit/{id?}")]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Admin/Categories/Edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Color,ImageFile,ImageTitle")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Define wwwrootpath, filename, extension
                    string wwwRootPath = _host.WebRootPath;
                    string file = Path.GetFileNameWithoutExtension(category.ImageFile.FileName);
                    string extension = Path.GetExtension(category.ImageFile.FileName);
                    //Make file name "unique"
                    file = file + DateTime.Now.ToString("yyyyMMddssff") + extension;

                    //Otherwise ImageName will be null in database
                    category.ImageName = file;

                    string ImagePath = Path.Combine(wwwRootPath, "Images", file);
                    //Upload to defined path with FileStream
                    using (var fileStream = new FileStream(ImagePath, FileMode.Create))
                    {
                        await category.ImageFile.CopyToAsync(fileStream);
                    }

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Category/Delete/5
        [Route("/Admin/Categories/Delete/{id?}")]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [Route("/Admin/Categories/Delete/{id?}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);

            //Delete the Image from folder
            var imagePath = Path.Combine(_host.WebRootPath, "Images", category.ImageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
