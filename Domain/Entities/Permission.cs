using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Permission
    {
        [Column("permission_id")]
        public Guid PermissionId { get; set; }
        [Required]
        [Column("permission_name")]
        [JsonPropertyName("permission_name")]
        public string PermissionName { get; set; }
        [JsonIgnore]
        public virtual ICollection<Role> Roles { get; set; }
    }
}
