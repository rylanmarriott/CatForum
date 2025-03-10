using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatForum.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("Discussion")]
        public int DiscussionId { get; set; }

        // Navigation property for the discussion the comment belongs to
        public virtual Discussion? Discussion { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }

        // Navigation property for the user who posted the comment
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
