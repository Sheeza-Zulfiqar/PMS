using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PMSApi.Enums;

namespace PMSApi.Entities
{
   
    public class ProjectTasks : BaseEntity
    {
        [Required]
        public int ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; } = default!;

        [Required, MaxLength(200)]
        public string Title { get; set; } = default!;

        [MaxLength(2000)]
        public string? Description { get; set; }
        public int Duration { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Finish { get; set; }

        public PMSApi.Enums.TaskStatus Status { get; set; } = PMSApi.Enums.TaskStatus.Pending; 
        public DateTime? CompletedAt { get; set; }                    

    }


}
