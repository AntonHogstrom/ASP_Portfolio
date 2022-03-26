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
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _host;

        public CourseController(ApplicationDbContext context, IWebHostEnvironment host)
        {
            _context = context;
            this._host = host;
        }

        // GET: Course
        [Route("/Admin/Courses")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Course.OrderByDescending(item => item.StartDate).ToListAsync());
        }

        // GET: Course/Details/5
        [Route("/Admin/Courses/Details/{id?}")]
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Course/Create
        [Route("/Admin/Courses/Create")]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Admin/Courses/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,Progression,Syllabus,StartDate,EndDate,ImageFile,ImageTitle")] Course course)
        {
            if (ModelState.IsValid)
            {

                //Define wwwrootpath, filename, extension
                string wwwRootPath = _host.WebRootPath;
                string file = Path.GetFileNameWithoutExtension(course.ImageFile.FileName);
                string extension = Path.GetExtension(course.ImageFile.FileName);
                //Make file name "unique"
                file = file + DateTime.Now.ToString("yyyyMMddssff") + extension;

                //Otherwise ImageName will be null in database
                course.ImageName = file;

                string ImagePath = Path.Combine(wwwRootPath, "Images", file);
                //Upload to defined path with FileStream
                using (var fileStream = new FileStream(ImagePath, FileMode.Create))
                {
                    await course.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Course/Edit/5
        [Route("/Admin/Courses/Edit/{id?}")]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Admin/Courses/Edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Progression,Syllabus,StartDate,EndDate,ImageFile,ImageTitle")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Define wwwrootpath, filename, extension
                    string wwwRootPath = _host.WebRootPath;
                    string file = Path.GetFileNameWithoutExtension(course.ImageFile.FileName);
                    string extension = Path.GetExtension(course.ImageFile.FileName);
                    //Make file name "unique"
                    file = file + DateTime.Now.ToString("yyyyMMddssff") + extension;

                    //Otherwise ImageName will be null in database
                    course.ImageName = file;

                    string ImagePath = Path.Combine(wwwRootPath, "Images", file);
                    //Upload to defined path with FileStream
                    using (var fileStream = new FileStream(ImagePath, FileMode.Create))
                    {
                        await course.ImageFile.CopyToAsync(fileStream);
                    }

                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            return View(course);
        }

        // GET: Course/Delete/5
        [Route("/Admin/Courses/Delete/{id?}")]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        // POST: Course/Delete/5
        [Route("/Admin/Courses/Delete/{id?}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);

            //Delete the Image from folder
            var imagePath = Path.Combine(_host.WebRootPath, "Images", course.ImageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}
