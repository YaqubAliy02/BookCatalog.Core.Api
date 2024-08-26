
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Role
    {
        [Column("role_id")]
        public Guid RoleId { get; set; }

        [Column("role_name")]
        public string RoleName { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
    }
}
