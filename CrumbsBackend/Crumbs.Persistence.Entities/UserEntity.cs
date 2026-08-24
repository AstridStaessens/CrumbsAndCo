using Crumbs.Persistence.Entities;
using System.Security.Cryptography.Pkcs;

namespace Crumbs.Persistence.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Klant";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();

    }
}
