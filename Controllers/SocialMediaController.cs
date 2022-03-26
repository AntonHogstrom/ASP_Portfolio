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
    public class SocialMediaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _host;

        public SocialMediaController(ApplicationDbContext context, IWebHostEnvironment host)
        {
            _context = context;
            this._host = host;
        }

        // GET: SocialMedia
        [Route("/Admin/SocialMedia")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.SocialMedia.OrderByDescending(item => item.Id).ToListAsync());
        }

        // GET: SocialMedia/Details/5
        [Route("/Admin/SocialMedia/Details/{id?}")]
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialMedia = await _context.SocialMedia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (socialMedia == null)
            {
                return NotFound();
            }

            return View(socialMedia);
        }

        // GET: SocialMedia/Create
        [Route("/Admin/SocialMedia/Create")]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: SocialMedia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Admin/SocialMedia/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title,Url,ImageFile,ImageTitle")] SocialMedia socialMedia)
        {
            if (ModelState.IsValid)
            {
                //Define wwwrootpath, filename, extension
                string wwwRootPath = _host.WebRootPath;
                string file = Path.GetFileNameWithoutExtension(socialMedia.ImageFile.FileName);
                string extension = Path.GetExtension(socialMedia.ImageFile.FileName);
                //Make file name "unique"
                file = file + DateTime.Now.ToString("yyyyMMddssff") + extension;

                //Otherwise ImageName will be null in database
                socialMedia.ImageName = file;

                string ImagePath = Path.Combine(wwwRootPath, "Images", file);
                //Upload to defined path with FileStream
                using (var fileStream = new FileStream(ImagePath, FileMode.Create))
                {
                    await socialMedia.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(socialMedia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(socialMedia);
        }

        // GET: SocialMedia/Edit/5
        [Route("/Admin/SocialMedia/Edit/{id?}")]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialMedia = await _context.SocialMedia.FindAsync(id);
            if (socialMedia == null)
            {
                return NotFound();
            }
            return View(socialMedia);
        }

        // POST: SocialMedia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Admin/SocialMedia/Edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Url,ImageFile,ImageTitle")] SocialMedia socialMedia)
        {
            if (id != socialMedia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Define wwwrootpath, filename, extension
                    string wwwRootPath = _host.WebRootPath;
                    string file = Path.GetFileNameWithoutExtension(socialMedia.ImageFile.FileName);
                    string extension = Path.GetExtension(socialMedia.ImageFile.FileName);
                    //Make file name "unique"
                    file = file + DateTime.Now.ToString("yyyyMMddssff") + extension;

                    //Otherwise ImageName will be null in database
                    socialMedia.ImageName = file;

                    string ImagePath = Path.Combine(wwwRootPath, "Images", file);
                    //Upload to defined path with FileStream
                    using (var fileStream = new FileStream(ImagePath, FileMode.Create))
                    {
                        await socialMedia.ImageFile.CopyToAsync(fileStream);
                    }
                    _context.Update(socialMedia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SocialMediaExists(socialMedia.Id))
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
            return View(socialMedia);
        }

        // GET: SocialMedia/Delete/5
        [Route("/Admin/SocialMedia/Delete/{id?}")]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var socialMedia = await _context.SocialMedia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (socialMedia == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(socialMedia);
        }

        // POST: SocialMedia/Delete/5
        [Route("/Admin/SocialMedia/Delete/{id?}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var socialMedia = await _context.SocialMedia.FindAsync(id);

            //Delete the Image from folder
            var imagePath = Path.Combine(_host.WebRootPath, "Images", socialMedia.ImageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.SocialMedia.Remove(socialMedia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SocialMediaExists(int id)
        {
            return _context.SocialMedia.Any(e => e.Id == id);
        }
    }
}
