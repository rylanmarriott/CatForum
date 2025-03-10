using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CatForum.Data;
using CatForum.Models;
using System.Threading.Tasks;

namespace CatForum.Controllers
{
    public class CommentsController : Controller
    {
        private readonly CatForumContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(CatForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,DiscussionId")] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Discussions", new { id = comment.DiscussionId });
            }

            comment.ApplicationUserId = _userManager.GetUserId(User);
            comment.CreatedDate = DateTime.UtcNow;

            _context.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Discussions", new { id = comment.DiscussionId });
        }
    }
}
