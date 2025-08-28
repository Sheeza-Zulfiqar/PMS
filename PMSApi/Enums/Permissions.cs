using PMSApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace PMSApi.Enums
{

    public enum AccessLevel
    {
        None = 0,
        Read = 1,
        ReadCreate = 2,
        ReadCreateUpdate = 3,
        ReadCreateUpdateDelete = 4
    }

 

    public class Permission : BaseEntity
    {
  
        [Required]
        public AccessLevel User { get; set; } = AccessLevel.None;

 
     }

}
