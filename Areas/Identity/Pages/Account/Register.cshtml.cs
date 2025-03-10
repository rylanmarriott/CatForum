using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CatForum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CatForum.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            [Required]
            [Display(Name = "Full Name")]
            public string Name { get; set; }

            [Display(Name = "Location")]
            public string? Location { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Passwords do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Profile Picture")] 
            public IFormFile? ProfilePicture { get; set; }
        }


        public async Task<IActionResult> OnPostAsync()
        {
            // Manually extract data from Request.Form
            var name = Request.Form["Input.Name"];
            var email = Request.Form["Input.Email"];
            var password = Request.Form["Input.Password"];
            var confirmPassword = Request.Form["Input.ConfirmPassword"];
            var profilePicture = Request.Form.Files["Input.ProfilePicture"];

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "All fields are required.");
                return Page();
            }

            // Default profile picture if none is uploaded
            string profileImageFileName = "placeholder.png";

            if (profilePicture != null && profilePicture.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(profilePicture.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }

                profileImageFileName = fileName;
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Name = name,
                Location = "", // Optional, can add later
                ImageFilename = profileImageFileName
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect("~/");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }




    }
}
