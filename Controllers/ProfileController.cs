using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatForum.Data;
using System.Threading.Tasks;

public class ProfileController : Controller
{
    private readonly CatForumContext _context;

    public ProfileController(CatForumContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string userId)
    {
        if (string.IsNullOrEmpty(userId)) return NotFound();

        var user = await _context.Users
            .Include(u => u.Discussions)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return NotFound();

        return View(user);
    }
}
