using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PerformanceManagementSystem.Areas.PerformanceManagement.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnGet(Guid timeId)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(30)
        };
        Response.Cookies.Append("PerformanceManagementCookie", timeId.ToString(), cookieOptions);
        return RedirectToPage("./Index");
    }
}