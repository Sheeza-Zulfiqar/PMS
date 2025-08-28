namespace PMSApi.DTOs.UserDtos
{
    public class UserReadDto : BaseReadDto
    {
        public string? MobileNumber { get; set; }
    
        public string? FullName { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public Guid? Image { get; set; }
        public int? RoleID { get; set; }
    }

}
