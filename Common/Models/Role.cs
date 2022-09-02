using Microsoft.AspNetCore.Identity;

namespace Common.Models
{
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
