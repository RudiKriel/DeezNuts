using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class MemberDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PhotoURL { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Age { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastActive { get; set; }

        public ICollection<PhotoDTO> Photos { get; set; }
    }
}
