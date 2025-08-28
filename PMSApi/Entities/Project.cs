using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PMSApi.Entities
{
    public class Project : BaseEntity
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = default!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = default!;

        public ICollection<ProjectTasks> ProjectTasks { get; set; } = new List<ProjectTasks>();
    }
}
