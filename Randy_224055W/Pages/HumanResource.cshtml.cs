using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Randy_224055W.Pages
{

    [Authorize(Policy = "MustBelongToHRDepartment", AuthenticationSchemes = "MyCookieAuth")]
    public class HumanResourceModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
