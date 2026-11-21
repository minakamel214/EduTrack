using System.ComponentModel.DataAnnotations;

namespace EduTrack.Models
{
    // يمثل تسليم الطالب لحل التاسك + تقييم ودرجة المحاضر
    public class Submission
    {
        public int Id { get; set; }

        [Display(Name = "تاريخ التسليم")]
        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        // مسار ملف الحل المرفوع من الطالب داخل wwwroot/uploads/submissions
        [Required]
        [Display(Name = "ملف الحل")]
        public string SolutionFilePath { get; set; }

        [Display(Name = "ملاحظات الطالب")]
        public string? StudentNotes { get; set; }

        // بيانات التقييم (يملأها المحاضر عند التصحيح)
        [Display(Name = "الدرجة")]
        public int? Grade { get; set; }

        [Display(Name = "تعليق المحاضر (Feedback)")]
        public string? InstructorFeedback { get; set; }

        // Pending (قيد المراجعة) أو Graded (تم التصحيح)
        [Display(Name = "الحالة")]
        public string Status { get; set; } = "Pending";

        // العلاقات
        public int AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }

        public int StudentId { get; set; }
        public Student? student { get; set; }
    }
}
