using System.ComponentModel.DataAnnotations;

namespace EduTrack.ViewModels
{
    public class CreateAssignmentViewModel
    {
        [Required]
        public int CourseId { get; set; }

        public string? CourseTitle { get; set; }

        [Required(ErrorMessage = "عنوان التكليف مطلوب")]
        [StringLength(200, ErrorMessage = "العنوان يجب ألا يزيد عن 200 حرف")]
        [Display(Name = "عنوان التكليف")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "الوصف / التعليمات")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "تاريخ التسليم مطلوب")]
        [DataType(DataType.DateTime)]
        [Display(Name = "موعد التسليم (Deadline)")]
        public DateTime Deadline { get; set; }

        [Required(ErrorMessage = "الدرجة العظمى مطلوبة")]
        [Range(1, 1000, ErrorMessage = "الدرجة يجب أن تكون بين 1 و 1000")]
        [Display(Name = "الدرجة العظمى")]
        public int MaxScore { get; set; }

        [Display(Name = "ملف مرفق (PDF)")]
        public IFormFile? AttachmentFile { get; set; }
    }
}