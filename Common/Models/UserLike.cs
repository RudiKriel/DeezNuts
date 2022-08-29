namespace Common.Models
{
    public class UserLike
    {
        public int SourceUserId { get; set; }
        public virtual User SourceUser { get; set; }

        public int LikedUserId { get; set; }
        public virtual User LikedUser { get; set; }
    }
}
