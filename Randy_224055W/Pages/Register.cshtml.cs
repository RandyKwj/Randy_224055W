using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Randy_224055W.Model;
using Randy_224055W.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Randy_224055W.Pages
{
    public class RegisterModel : PageModel
    {

        private UserManager<Register> userManager { get; }
        private IDataProtector _protector { get; }
        private SignInManager<Register> signInManager { get; }


        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public Register RModel { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        public IFormFile? UploadResume { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(12, ErrorMessage = "Password must be at least 12 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{12,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
        public string Password { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

        public RegisterModel(UserManager<Register> userManager,
        SignInManager<Register> signInManager,
         IDataProtectionProvider protector, IWebHostEnvironment webHostEnvironment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _protector = protector.CreateProtector("protectnric");
            this.webHostEnvironment = webHostEnvironment;
        }




        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (UploadResume == null)
                {
                    return Page();
                }
                var fileExtension = Path.GetExtension(UploadResume.FileName).ToLower();
                if (fileExtension != ".pdf" && fileExtension != ".docx")
                {
                    ModelState.AddModelError("RModel.Resume", "PDF or Word document only");
                    return Page();
                }

                var uploadsFolder = "uploads";
                var filename = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot", uploadsFolder, filename);

                var resumeName = string.Format("/{0}/{1}", uploadsFolder, filename);

                var protectedNRIC = _protector.Protect(RModel.NRIC);
                var user = new Register()
                {
                    Name = RModel.Name,
                    UserName = Email,
                    Email = Email,
                    DateOfBirth = RModel.DateOfBirth,
                    WhoAmI = System.Net.WebUtility.HtmlEncode(RModel.WhoAmI),
                    NRIC = protectedNRIC,
                    Resume = resumeName,
                    Gender = RModel.Gender,
                };
                HttpContext.Session.SetString("email", user.Email);

                var result = await userManager.CreateAsync(user, Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    await UploadResume.CopyToAsync(fileStream);
                    return RedirectToPage("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }


    }
}
