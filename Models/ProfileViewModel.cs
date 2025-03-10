using System.Collections.Generic;

namespace CatForum.Models
{
    public class ProfileViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string? Location { get; set; }
        public string? ImageFilename { get; set; }
        public List<Discussion> Discussions { get; set; } = new List<Discussion>();
    }
}
