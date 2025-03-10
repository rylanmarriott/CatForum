using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace CatForum.Models
{
    public class Discussion
    {
        [Key]
        public int DiscussionId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string? ImageFilename { get; set; } = "/images/placeholder.png"; // Default image

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        // Ensure the ApplicationUserId field exists
        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }

        // Navigation property for the User who created the discussion
        public virtual ApplicationUser? ApplicationUser { get; set; }

        // Navigation property for Comments
        public virtual List<Comment>? Comments { get; set; } = new List<Comment>();

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
