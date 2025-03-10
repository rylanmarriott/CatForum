using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatForum.Data;
using CatForum.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace CatForum.Controllers
{
    [Authorize] 
    public class DiscussionsController : Controller
    {
        private readonly CatForumContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DiscussionsController(CatForumContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Discussions
        public async Task<IActionResult> Index()
        {
            var discussions = await _context.Discussion
                .Include(d => d.ApplicationUser)
                .OrderByDescending(d => d.CreateDate)
                .ToListAsync();
            return View(discussions);
        }

        // GET: Discussions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Discussions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,ImageFile")] Discussion discussion)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    discussion.ApplicationUserId = user.Id;
                }

                discussion.CreateDate = DateTime.UtcNow;

                if (discussion.ImageFile != null && discussion.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(discussion.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await discussion.ImageFile.CopyToAsync(fileStream);
                    }

                    discussion.ImageFilename =  uniqueFileName; // Ensure correct path
                }
                else
                {
                    discussion.ImageFilename = "placeholder.png"; // Default image
                }

                _context.Add(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(discussion);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var discussion = await _context.Discussion.FindAsync(id);
            if (discussion == null) return NotFound();

            if (discussion.ApplicationUserId != _userManager.GetUserId(User)) { return Forbid(); }
            return View(discussion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content,ImageFile,ImageFilename")] Discussion discussion)
        {
            if (id != discussion.DiscussionId) return NotFound();

            var existingDiscussion = await _context.Discussion.FindAsync(id);
            if (existingDiscussion == null) return NotFound();

            if (existingDiscussion.ApplicationUserId != _userManager.GetUserId(User))
                return Forbid();

            existingDiscussion.Title = discussion.Title;
            existingDiscussion.Content = discussion.Content;

            if (discussion.ImageFile != null && discussion.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(discussion.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await discussion.ImageFile.CopyToAsync(fileStream);
                }

                // Delete old image if it exists and isn't the placeholder
                if (!string.IsNullOrEmpty(existingDiscussion.ImageFilename) && !existingDiscussion.ImageFilename.EndsWith("placeholder.png"))
                {
                    string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingDiscussion.ImageFilename.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                existingDiscussion.ImageFilename =  uniqueFileName; // Ensure correct path
            }
            else if (string.IsNullOrEmpty(existingDiscussion.ImageFilename) || existingDiscussion.ImageFilename == "placeholder.png")
            {
                existingDiscussion.ImageFilename = "placeholder.png";
            }

            _context.Update(existingDiscussion);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var discussion = await _context.Discussion
                .Include(d => d.ApplicationUser) // Include the user who created the discussion
                .Include(d => d.Comments)
                .ThenInclude(c => c.ApplicationUser) // Include the users who made comments
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null) return NotFound();

            return View(discussion);
        }




        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var discussion = await _context.Discussion.FindAsync(id);
            if (discussion == null) return NotFound();

            if (discussion.ApplicationUserId != _userManager.GetUserId(User)) return Forbid(); 

            return View(discussion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussion = await _context.Discussion.FindAsync(id);
            if (discussion == null) return NotFound();

            if (discussion.ApplicationUserId != _userManager.GetUserId(User)) { return Forbid(); } 

            _context.Discussion.Remove(discussion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize] 
        public async Task<IActionResult> Manage()
        {
            var userId = _userManager.GetUserId(User); 
            var discussions = await _context.Discussion
                .Where(d => d.ApplicationUserId == userId)
                .OrderByDescending(d => d.CreateDate)
                .ToListAsync();

            return View(discussions);
        }
    }
}
