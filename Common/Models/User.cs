
namespace Common.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;

        public int GetAge()
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;

            if (DateOfBirth.Date > today.AddYears(-age))
                age--;

            return age;
        }

        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<UserLike> LikedByUsers { get; set; }
        public virtual ICollection<UserLike> LikedUsers { get; set; }
    }
}
