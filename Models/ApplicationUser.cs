using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EduTrack.Models
{
    // يمثل جدول المستخدمين الأساسي (AspNetUsers) المسؤول عن تسجيل الدخول والصلاحيات
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "الاسم بالكامل مطلوب")]
        [Display(Name = "الاسم بالكامل")]
        public string FullName { get; set; }
        public string Address { get; internal set; }
    }
}
