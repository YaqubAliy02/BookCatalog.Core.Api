namespace Application.DTOs.RoleDTO
{
    public class RoleGetDTO
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public Guid[] PermissionsId { get; set; }
    }
}
