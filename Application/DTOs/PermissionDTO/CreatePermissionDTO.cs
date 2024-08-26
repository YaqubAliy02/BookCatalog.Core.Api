
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.PermissionDTO
{
    public class CreatePermissionDTO
    {
        [Required]
        public string PermissionName { get; set; }
    }
}
