using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public bool IsMain { get; set; }
        public bool IsApproved { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}