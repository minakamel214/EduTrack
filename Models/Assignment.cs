using System.ComponentModel.DataAnnotations;

namespace EduTrack.Models
{
    // يمثل التاسك (الواجب العملي) الذي يرفعه المحاضر داخل الكورس
    public class Assignment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "عنوان التاسك مطلوب")]
        [Display(Name = "عنوان التاسك")]
        public string Title { get; set; }

        [Required(ErrorMessage = "تفاصيل التاسك مطلوبة")]
        [Display(Name = "شرح المطلوب")]
        public string Description { get; set; }

        [Required(ErrorMessage = "آخر موعد للتسليم مطلوب")]
        [Display(Name = "آخر موعد للتسليم (Deadline)")]
        public DateTime Deadline { get; set; }

        [Range(1, 1000)]
        [Display(Name = "الدرجة النهائية")]
        public int MaxDegree { get; set; } = 100;

        // مسار ملف الـ PDF المرفق من المحاضر داخل wwwroot/uploads/assignments
        [Display(Name = "ملف شرح التاسك (PDF)")]
        public string? AttachmentFilePath { get; set; }

        // الكورس التابع له هذا التاسك
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        // جميع تسليمات الطلاب لهذا التاسك
        public List<Submission> Submissions { get; set; } = new List<Submission>();
    }
}
