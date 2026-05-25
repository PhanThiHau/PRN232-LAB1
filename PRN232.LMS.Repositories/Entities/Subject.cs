using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN232.LMS.Repositories.Entities
{
    [Table("Subjects")]
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectId { get; set; }

        [Required]
        [MaxLength(20)]
        public string SubjectCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string SubjectName { get; set; } = string.Empty;

        [Required]
        public int Credit { get; set; }
    }
}
