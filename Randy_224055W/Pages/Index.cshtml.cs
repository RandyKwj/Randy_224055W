using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Randy_224055W.ViewModels;

namespace Randy_224055W.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IDataProtector _protector;
        private readonly UserManager<Register> _userManager;

        public IndexModel(ILogger<IndexModel> logger, IDataProtectionProvider protector, UserManager<Register> userManager)
        {
            _logger = logger;
            _protector = protector.CreateProtector("protectnric");
            _userManager = userManager;
        }

        public Register User { get; set; }

        public string ProtectedNRIC { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            User = await _userManager.Users.FirstAsync(u => u.Id == userId);

            ProtectedNRIC = User.NRIC;

            var unprotectedNRIC = _protector.Unprotect(User.NRIC);
            User.NRIC = unprotectedNRIC;

            return Page();
        }
    }
}