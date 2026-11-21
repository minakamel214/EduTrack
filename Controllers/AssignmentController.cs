using EduTrack.Data;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class AssignmentsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public AssignmentsController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

   

    [HttpGet]
    [Authorize(Roles = "Instructor,Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpGet]
    [Authorize(Roles = "Instructor,Admin")]
    public IActionResult Create(int courseId)
    {
        var course = _context.Courses
            .AsNoTracking()
            .FirstOrDefault(c => c.Id == courseId);

        if (course == null)
        {
            return NotFound();
        }

        var model = new CreateAssignmentViewModel
        {
            CourseId = course.Id,
            CourseTitle = course.Title, 
            Deadline = DateTime.Now.AddDays(7)
        };

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Instructor,Admin")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateAssignmentViewModel model, IFormFile? attachmentFile)
    {
        string? uniqueFileName = null;

        if (attachmentFile != null && attachmentFile.Length > 0)
        {
            string extension = Path.GetExtension(attachmentFile.FileName).ToLower();
            if (extension != ".pdf")
            {
                ModelState.AddModelError("AttachmentFile", "يجب أن يكون الملف بصيغة PDF");
                return View(model);
            }

            if (!ModelState.IsValid)
                return View(model);

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "assignments");
            Directory.CreateDirectory(uploadsFolder);

            uniqueFileName = Guid.NewGuid().ToString() + extension;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.AttachmentFile.CopyTo(fileStream);
            }
        }
        else if (!ModelState.IsValid)
        {
            return View(model);
        }

        _context.Assignments.Add(new Assignment
        {
            Title = model.Title,
            Description = model.Description,
            Deadline = model.Deadline,
            MaxDegree = model.MaxScore,
            CourseId = model.CourseId,
            AttachmentFilePath = uniqueFileName
        });

    

        _context.SaveChanges();
        return RedirectToAction("Details", "Courses", new { id = model.CourseId });
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var assignment = _context.Assignments
            .Include(a => a.Course)
            .Include(a => a.Submissions)
                .ThenInclude(s => s.student)
            .FirstOrDefault(a => a.Id == id);

        if (assignment == null)
            return NotFound();

        return View(assignment);
    }
}