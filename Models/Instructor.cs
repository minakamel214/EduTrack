using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrack.Models
{
    // يمثل بيانات المحاضر ويرتبط بحساب دخول (ApplicationUser) وصورة شخصية
    public class Instructor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المحاضر مطلوب")]
        [Display(Name = "اسم المحاضر")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }


        [Required(ErrorMessage = "التخصص مطلوب")]
        [Display(Name = "التخصص")]
        public string Specialization { get; set; }


        [Display(Name = "نبذة عن المحاضر")]
        public string? Bio { get; set; }

        // مسار حفظ الصورة داخل wwwroot/images/instructors
        [Display(Name = "الصورة الشخصية")]
        public string? ImagePath { get; set; }

        // الربط مع جدول AspNetUsers (One-to-One)
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        // المحاضر الواحد يمكنه تدريس أكثر من كورس
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
