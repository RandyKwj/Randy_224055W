using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Randy_224055W.ViewModels;

namespace Randy_224055W.Pages
{
	public class LogoutModel : PageModel
	{

		private readonly SignInManager<Register> signInManager;
		public LogoutModel(SignInManager<Register> signInManager)
		{
			this.signInManager = signInManager;
		}
		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostLogoutAsync()
		{
			await signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToPage("Login");
		}
		public async Task<IActionResult> OnPostDontLogoutAsync()
		{
			return RedirectToPage("Index");
		}


	}
}
