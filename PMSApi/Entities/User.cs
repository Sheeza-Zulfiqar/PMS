using System.ComponentModel.DataAnnotations;

namespace PMSApi.Entities
{
    public class User:BaseEntity
    {
        [Required, MaxLength(150)]
        public string Username { get; set; } = default!;

        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = default!;

         [Required]
        public string Password { get; set; } = default!;
        public string? MobileNumber { get; set; }
 
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public string? Lastname { get; set; }
        public string? Firstname { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
