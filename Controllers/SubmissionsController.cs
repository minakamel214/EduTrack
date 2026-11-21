using EduTrack.Data;
using EduTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EduTrack.Controllers
{
    [Authorize]
    public class SubmissionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public SubmissionsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public IActionResult Submit(int assignmentId)
        {
            var assignment = _context.Assignments.FirstOrDefault(a => a.Id == assignmentId);
            if (assignment == null)
                return NotFound();

            ViewBag.AssignmentId = assignment.Id;
            ViewBag.AssignmentTitle = assignment.Title;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(int assignmentId, IFormFile? solutionFile)
        {
            var assignment = _context.Assignments.FirstOrDefault(a => a.Id == assignmentId);
            if (assignment == null)
                return NotFound();

            if (solutionFile == null || solutionFile.Length == 0)
            {
                TempData["ErrorMessage"] = "الرجاء تحميل ملف الحل.";
                ViewBag.AssignmentId = assignment.Id;
                ViewBag.AssignmentTitle = assignment.Title;
                return View();
            }

            var allowedExtensions = new[] { ".zip", ".rar", ".pdf" };
            var extension = Path.GetExtension(solutionFile.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                TempData["ErrorMessage"] = "يجب أن يكون الملف بصيغة .zip أو .rar أو .pdf فقط.";
                ViewBag.AssignmentId = assignment.Id;
                ViewBag.AssignmentTitle = assignment.Title;
                return View();
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "submissions");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                solutionFile.CopyTo(fileStream);
            }

            var studentId = _userManager.GetUserId(User);
            var student = _context.Students.FirstOrDefault(s => s.UserId == studentId);

            var submission = new Submission
            {
                AssignmentId = assignment.Id,
                StudentId = student.Id,
                SolutionFilePath = uniqueFileName,
                Status = "Pending",
                SubmittedAt = DateTime.Now
            };

            _context.Submissions.Add(submission);
            _context.SaveChanges();

            return RedirectToAction(nameof(MySubmissions));
        }


        [HttpGet]
        [Authorize(Roles = "Instructor,Admin")]
        public IActionResult Grade(int Id) 
        {

            var submission = _context.Submissions
                .Include(s => s.Assignment)
                .Include(s => s.student)
                .FirstOrDefault(s => s.Id == Id);
            return View(submission);
        }

        [HttpPost]
        [Authorize(Roles = "Instructor,Admin")]
        public IActionResult Grade(int Id, int grade, string feedback)
        {
            var submission = _context.Submissions.FirstOrDefault(s => s.Id == Id);
            if (submission == null)
                return NotFound();

            submission.Grade = grade;
            submission.InstructorFeedback = feedback;
            submission.Status = "Graded";

            _context.SaveChanges();

            return RedirectToAction(nameof(Grade), new { Id });
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public IActionResult MySubmissions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var submissions = _context.Submissions
                .Include(s => s.Assignment)
                    .ThenInclude(a => a.Course)
                .Where(s => s.student.UserId == userId)
                .ToList();

            return View(submissions);
        }
    }
}
