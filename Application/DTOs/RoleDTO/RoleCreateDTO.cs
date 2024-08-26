namespace Application.DTOs.RoleDTO
{
    public class RoleCreateDTO
    {
        public string RoleName { get; set; }
        public Guid[] PermissionId { get; set; }
    }
}
