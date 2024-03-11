using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Projectiv.AuthService.Host.Pages;

public class Welcome : PageModel
{
    public class WelcomeModel : PageModel
    {
        public string Username { get; set; }

        public void OnGet()
        {
            Username = User.Identity.Name;
        }
    }
}