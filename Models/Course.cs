using System.ComponentModel.DataAnnotations;

namespace EduTrack.Models
{
    // يمثل الكورس التدريبي وهو المحور الأساسي الذي يجمع المحاضر والطلاب والتاسكات
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "عنوان الكورس مطلوب")]
        [Display(Name = "اسم الكورس")]
        public string Title { get; set; }

        [Required(ErrorMessage = "وصف الكورس مطلوب")]
        [Display(Name = "وصف الكورس")]
        public string Description { get; set; }

        [Range(1, 500, ErrorMessage = "عدد الساعات يجب أن يكون بين 1 و 500")]
        [Display(Name = "عدد الساعات")]
        public int TotalHours { get; set; }

        // مسار صورة الغلاف داخل wwwroot/images/courses
        [Display(Name = "صورة الغلاف")]
        public string? ThumbnailPath { get; set; }

        // المحاضر المسؤول عن الكورس
        [Display(Name = "المحاضر")]
        public int InstructorId { get; set; }
        public Instructor? Instructor { get; set; }

        // العلاقات: الطلاب المسجلون في الكورس + التاسكات الخاصة بالكورس
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
