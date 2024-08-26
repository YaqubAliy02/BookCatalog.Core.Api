namespace Application.DTOs.RoleDTO
{
    public class RoleUpdateDTO
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public Guid[] PermissionsId { get; set; }
    }
}
