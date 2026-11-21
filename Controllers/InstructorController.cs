using EduTrack.Data;
using EduTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Controllers
{
    [Authorize(Roles = "Admin")]

    public class InstructorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public InstructorController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var instructors = _context.Instructors
                .Include(i => i.User)
                .Include(i => i.Courses)
                .ToList();

            return View(instructors);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Instructor instructor, IFormFile? ImageFile, IWebHostEnvironment webHostEnvironment)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = instructor.Email,
                    Email = instructor.Email,
                    FullName = instructor.FullName,
                    Address = string.Empty
                };

                var createUser = _userManager.CreateAsync(user, "Password1234").GetAwaiter().GetResult();
                var roleAssignment = _userManager.AddToRoleAsync(user, "Instructor").GetAwaiter().GetResult();

                string? savedFileName = null;

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "instructors");
                    Directory.CreateDirectory(uploadsFolder);

                    string extension = Path.GetExtension(ImageFile.FileName);
                    savedFileName = Guid.NewGuid().ToString() + extension;
                    string filePath = Path.Combine(uploadsFolder, savedFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(fileStream);
                    }
                }

                var instructorEntity = new Instructor
                {
                    FullName = instructor.FullName,
                    Email = instructor.Email,
                    Specialization = instructor.Specialization,
                    Bio = instructor.Bio,
                    UserId = user.Id,
                    ImagePath = savedFileName
                };

                _context.Instructors.Add(instructorEntity);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var instructor = _context.Instructors
                .Include(i => i.Courses) 
                .FirstOrDefault(i => i.Id == id);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

    }
}