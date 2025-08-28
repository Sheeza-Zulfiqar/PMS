using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PMSApi.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; }

        [Required]
        public DateTime? UpdatedAt  { get; set; } 

        public int? CreateUserID { get; set; }
        public int? UpdateUserID { get; set; }

 
        [ForeignKey(nameof(CreateUserID))]
        public User? CreatedBy { get; set; }

        [ForeignKey(nameof(UpdateUserID))]
        public User? UpdatedBy { get; set; }

    }

}
