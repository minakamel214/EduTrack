using EduTrack.Data;
using EduTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string UniqueCourseFileName;
        public CoursesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            


            if (User.IsInRole("Admin"))
            {
                var AllCourses = _context.Courses.ToList();
                return View(AllCourses);
            }
            else if (User.IsInRole("Instructor"))
            {
                var userId = _userManager.GetUserId(User);
                var instructor = _context.Instructors.FirstOrDefault(i => i.UserId == userId);
                if (instructor != null)
                {
                    var instructorCourses = _context.Courses
                        .Where(c => c.InstructorId == instructor.Id)
                        .ToList();
                    return View(instructorCourses);
                }
            }
            else if (User.IsInRole("Student"))
            {
                var userId = _userManager.GetUserId(User);
                var student = _context.Students.FirstOrDefault(s => s.UserId == userId);
                if (student != null)
                {
                    var studentCourses = _context.Courses
                        .Where(c => c.Students.Any(s => s.Id == student.Id))
                        .ToList();
                    return View(studentCourses);
                }
            }
            return View(new List<Course>());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Course course, IFormFile? ThumbnailFile)
        {
            if (ModelState.IsValid)
            {
                if (ThumbnailFile != null && ThumbnailFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "courses");
                    Directory.CreateDirectory(uploadsFolder);

                    string extension = Path.GetExtension(ThumbnailFile.FileName);
                    string uniqueCourseFileName = Guid.NewGuid().ToString() + extension;
                    string filePath = Path.Combine(uploadsFolder, uniqueCourseFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        ThumbnailFile.CopyTo(fileStream);
                    }

                    course.ThumbnailPath = uniqueCourseFileName;
                }

                _context.Courses.Add(course);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Instructors = _context.Instructors.ToList();
            return View(course);
        }
            
        

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewBag.Instructors = _context.Instructors.ToList();
            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, Course course, IFormFile? ThumbnailFile)
        {
            var existingCourse = _context.Courses
                    .AsNoTracking()
                    .FirstOrDefault(c => c.Id == id);

            if (existingCourse == null)
            {
                return NotFound();
            }

            if (ThumbnailFile != null)
            {
                var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "courses", existingCourse.ThumbnailPath);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                var folder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "courses");
                var fileName = Guid.NewGuid() + Path.GetExtension(ThumbnailFile.FileName);
                var newPath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    ThumbnailFile.CopyTo(stream);
                }

                course.ThumbnailPath = fileName;
            }
            else
            {
                course.ThumbnailPath = existingCourse.ThumbnailPath;
            }

            _context.Courses.Update(course);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));


        }
        [HttpGet]
       public IActionResult Details(int id)
        {
            var course = _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Students)
                .Include(c => c.Assignments)
                .Include(c => c.Assignments)
                .FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }
    }
}
