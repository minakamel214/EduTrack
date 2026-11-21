using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrack.Models
{
    // يمثل بيانات الطالب ويرتبط بالكورس المسجل فيه وبحساب الدخول (ApplicationUser)
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الطالب مطلوب")]
        [Display(Name = "اسم الطالب")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; }

        [Display(Name = "الجامعة / الكلية")]
        public string? University { get; set; }

        // الكورس المسجل فيه الطالب مباشرة
        [Display(Name = "الكورس")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        // الربط مع جدول AspNetUsers (One-to-One)
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        // قائمة حلول التاسكات التي قام الطالب بتسليمها
        public List<Submission> Submissions { get; set; } = new List<Submission>();
    }
}
