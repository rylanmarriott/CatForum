using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CatForum.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.IO;

namespace CatForum.Areas.Identity.Pages.Account
{
    public class ManageProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ManageProfileModel(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Name { get; set; }

            public string Location { get; set; }

            public IFormFile ImageFile { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            Input = new InputModel
            {
                Name = user.Name,
                Location = user.Location
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (ModelState.IsValid)
            {
                user.Name = Input.Name;
                user.Location = Input.Location;

                if (Input.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Input.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Input.ImageFile.CopyToAsync(fileStream);
                    }

                    user.ImageFilename = uniqueFileName;
                }

                await _userManager.UpdateAsync(user);
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}
