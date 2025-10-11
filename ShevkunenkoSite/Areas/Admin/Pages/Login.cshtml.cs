using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ShevkunenkoSite.Areas.Admin.Pages;

public class LoginModel : PageModel
{
    private readonly IAccessRepository _accessContext;

    public LoginModel(IAccessRepository accessContext) => _accessContext = accessContext;

    [BindProperty]
    public AccessModel Access { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl)
    {
        if (ModelState.IsValid)
        {
            bool isAccessInDatabase = await _accessContext.Accesses
                .Where(u => u.Email == Access.Email && u.Password == Access.Password)
                .AnyAsync();

            if (isAccessInDatabase)
            {
                AccessModel access = await _accessContext.Accesses
                    .FirstAsync(u => u.Email == Access.Email && u.Password == Access.Password);

                Authenticate(access.Email); // ��������������

                //returnUrl = "/Admin";

                return Redirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError("Access.Password", "������������ ����� �(���) ������");

                return Page();
            }
        }

        return Page();
    }

    private void Authenticate(string userName)
    {
        // ������� ���� claim
        var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

        // ������� ������ ClaimsIdentity
        ClaimsIdentity id = new(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

        // ��������� ������������������ ����
        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }

    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToPage("/Index", new { area = "" });
    }
}
