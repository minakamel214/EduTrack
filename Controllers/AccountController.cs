using EduTrack.Data;
using EduTrack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult setupSystem()
        {
        IdentityRole r1  = new IdentityRole();
            r1.Name = "Admin";
            _roleManager.CreateAsync(r1).Wait();

            IdentityRole r2 = new IdentityRole();
            r2.Name = "Student";
            _roleManager.CreateAsync(r2).Wait();

            IdentityRole r3 = new IdentityRole();
            r3.Name = "Instructor";
            _roleManager.CreateAsync(r3).Wait();


            ApplicationUser admin = new ApplicationUser();
            admin.FullName = "CEO";
            admin .Address = "Cairo"; 
            admin.Email = "admin@example.com";
            admin.UserName = "admin@example.com";

            var result = _userManager.CreateAsync(admin, "Admin@123").Result;

            if(result.Succeeded)
            {
                _userManager.AddToRoleAsync(admin, "Admin").Wait();
            }
            return Content("System setup completed successfully.");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(string fullName, string email, string password, string address)
        {
            var user = new ApplicationUser
            {
                FullName = fullName,
                Email = email,
                UserName = email,
                Address = address
            };

           
            var result = _userManager.CreateAsync(user, password).Result;
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "Student").Wait();

                var student = new Student()
                {
                    UserId = user.Id,
                    FullName = user.UserName,
                    
                };

                _context.Students.Add(student);
                _context.SaveChanges();

                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.ErrorMessage = "Registration failed. Please try again.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var result = _signInManager.PasswordSignInAsync(email, password, false, false).Result;  
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View();
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().Wait();
            return RedirectToAction("Login", "Account");
        }   
    }
}
