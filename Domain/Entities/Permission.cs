using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Permission
    {
        [Column("permission_id")]
        public int PermissionId { get; set; }

        [Column("permission_name")]
        [JsonPropertyName("permission_name")]
        public string PermissionName { get; set; }
        public virtual ICollection<Role> Roles { get; set; }

    }
}
