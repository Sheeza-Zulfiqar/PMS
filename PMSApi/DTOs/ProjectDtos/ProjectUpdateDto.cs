using System.ComponentModel.DataAnnotations;

namespace PMSApi.DTOs.ProjectDtos
{
    public class ProjectUpdateDto
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = default!;
        [MaxLength(1000)]
        public string? Description { get; set; }
    }
}
