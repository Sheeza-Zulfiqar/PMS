using System.ComponentModel.DataAnnotations;

namespace PMSApi.DTOs.ProjectTasksDtos
{
    public class ProjectTaskUpdateDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = default!;
        [MaxLength(2000)]
        public string? Description { get; set; }
        public int Duration { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Finish { get; set; }
        public PMSApi.Enums.TaskStatus Status { get; set; }
 
    }
}
