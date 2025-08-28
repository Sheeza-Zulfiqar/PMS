using System.ComponentModel.DataAnnotations;

namespace PMSApi.Enums
{
    public enum TaskStatus
    {
        Pending = 0,

        [Display(Name = "To Do")]
        ToDo = 1,

        [Display(Name = "In Progress")]
        InProgress = 2,

        [Display(Name = "In Review")]
        InReview = 3,

        Done = 4
    }
}
