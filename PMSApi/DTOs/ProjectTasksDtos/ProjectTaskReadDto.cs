using Microsoft.OpenApi.Extensions;

namespace PMSApi.DTOs.ProjectTasksDtos
{
    public class ProjectTaskReadDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Finish { get; set; }
        public DateTime CreatedAt { get; set; }
        public PMSApi.Enums.TaskStatus Status { get; set; }
        public string StatusText => Status.GetDisplayName();
        public DateTime? CompletedAt { get; set; }
    }
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attr = fi?.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false)
                         .Cast<System.ComponentModel.DataAnnotations.DisplayAttribute>()
                         .FirstOrDefault();
            return attr?.GetName() ?? value.ToString();
        }
    }

}
