using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatForum.Data;
using CatForum.Models;

namespace CatForum.Controllers
{
    public class CommentsController : Controller
    {
        private readonly CatForumContext _context;

        public CommentsController(CatForumContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> TestAddComment()
        {
            var comment = new Comment
            {
                Content = "Test Comment",
                DiscussionId = 1,
                CreatedDate = DateTime.UtcNow
            };

            try
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                Console.WriteLine("Test Comment successfully added.");
            }
            catch (Exception ex) { Console.WriteLine($"Error saving test comment: {ex.Message}"); }
            return Ok("Test completed.");
        }


        // GET: Comments/Create
        public IActionResult Create(int discussionId)
        {
            ViewBag.DiscussionId = discussionId; // Pass DiscussionId to the view
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,DiscussionId")] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = new { success = false, message = "Validation failed", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() };

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") 
                {
                    return Json(errorResponse);
                }
                else
                {
                    TempData["ErrorMessage"] = string.Join(", ", errorResponse.errors);
                    return RedirectToAction("Details", "Discussions", new { id = comment.DiscussionId });
                }
            }

            try
            {
                comment.CreatedDate = DateTime.UtcNow;
                _context.Add(comment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") 
                {
                    return Json(new { success = false, message = ex.Message });
                }
                else
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return RedirectToAction("Details", "Discussions", new { id = comment.DiscussionId });
                }
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") 
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("Details", "Discussions", new { id = comment.DiscussionId });
            }
        }



    }
}
