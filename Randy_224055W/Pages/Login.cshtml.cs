using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Randy_224055W.ViewModels;
using System.Runtime;
using Microsoft.Extensions.Options;
using Randy_224055W.Model;

namespace Randy_224055W.Pages
{
    public class LoginModel : PageModel
    {

        private readonly SignInManager<Register> signInManager;

        private GoogleCaptchaConfig _settings;

        [BindProperty]
        public Login LModel { get; set; }

        public LoginModel(SignInManager<Register> signInManager, IOptions<GoogleCaptchaConfig> settings)
        {
            this.signInManager = signInManager;
            _settings = settings.Value;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Google recaptcha
            //GResponse _GooglereCaptcha = await VerifyCaptcha(LModel.Token);
            //if (!_GooglereCaptcha.success && _GooglereCaptcha.score <= 0.5)
            //{
            //    ModelState.AddModelError("", "You are not human!");
            //    return Page();
            //}
            if (ModelState.IsValid)
            {
                var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password, LModel.RememberMe, true);
                if (identityResult.Succeeded)
                {

                    HttpContext.Session.SetString("email", LModel.Email);
                    return RedirectToPage("Index");
                }
                ModelState.AddModelError("", "Username or Password incorrect");
            }
            return Page();
        }

        public virtual async Task<GResponse> VerifyCaptcha(string _Token)
        {
            GreCaptchaData data = new GreCaptchaData
            {
                response = _Token,
                secret = _settings.SiteKey
            };

            HttpClient client = new HttpClient();

            var response = await client.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={data.secret}&response={data.response}");

            var captchaResponse = response.Content.ReadFromJsonAsync<GResponse>().Result;


            return captchaResponse;

        }

        
    }
}

public class GResponse
{
    public bool success { get; set; }

    public double score { get; set; }

    public string action { get; set; }

    public DateTime challenge_ts { get; set; }

    public string hostname { get; set; }
}

public class GreCaptchaData
{
    public string response { get; set; } // token

    public string secret { get; set; }
}

