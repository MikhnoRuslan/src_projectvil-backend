using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projectiv.AuthService.Domain.Interfaces;
using Projectiv.AuthService.DomainShared.Configurations;
using Projectvil.Shared.EntityFramework.Models;
using Projectvil.Shared.Infrastructures.Constants;

namespace Projectiv.AuthService.Host.Pages;

[AllowAnonymous]
[IgnoreAntiforgeryToken]
public class Login : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IIdentityRepository _identityRepository;

    public Login(SignInManager<ApplicationUser> signInManager,
        IIdentityRepository identityRepository)
    {
        _signInManager = signInManager;
        _identityRepository = identityRepository;
    }
    
    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    
    public void OnGet()
    {
        
    }
    
    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid) return Page();
        var settings = ProjectivAuthConfiguration.BindSettings();
        var tokenEndpoint = settings.App.SelfUrl + "/connect/token";

        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"grant_type", "password"},
            {"client_id", "swagger-client-id"},
            {"scope", $"{ProjectivConstants.Api.AuthApi} {ProjectivConstants.Api.IdentityApi} {ProjectivConstants.Api.PetProjectApi} {ProjectivConstants.Api.GetWayApi} {ProjectivConstants.Api.NotificationApi}"},
            {"client_secret", "ProjectivSecret"},
            {"username", Input.Email},
            {"password", Input.Password}
        });

        var response = await client.SendAsync(request);
                
        if (response.IsSuccessStatusCode)
        {
            var user = await _identityRepository.GetAsync(x => x.Email.Equals(Input.Email));
            await _signInManager.PasswordSignInAsync(user.UserName!, Input.Password, false, lockoutOnFailure: false);

            return RedirectToPage("Welcome");
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        ModelState.AddModelError(string.Empty, $"Error from token endpoint: {errorContent}");
        return Page();
    }
}