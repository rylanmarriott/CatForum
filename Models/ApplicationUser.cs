using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace CatForum.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public string? Location { get; set; }

        public string? ImageFilename { get; set; } 

        [NotMapped]
        public IFormFile? ImageFile { get; set; } 

        public virtual ICollection<Discussion> Discussions { get; set; } = new List<Discussion>();
    }
}
