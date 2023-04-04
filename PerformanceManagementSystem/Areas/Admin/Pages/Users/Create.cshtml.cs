using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views;
using PerformanceManagementSystem.Data.Views.Positions;
using PerformanceManagementSystem.Data.Views.Users;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Users;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    public CreateModel(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult OnGet()
    {
        ViewData["Managers"] = new SelectList(_context.Users.Where(a => a.Active).Select(a => new UserResponseDto
        {
            Id = a.Id,
            Name = $"{a.Firstname} {a.Lastname} ({a.Email})"
        }), "Id", "Name");

        ViewData["PositionId"] = new SelectList(_context.Positions.Where(a => a.Active && !a.Deleted).Select(a => new PositionResponseDto
        {
            Title = a.Title,
            Id = a.Id
        }), "Id", "Title");
        return Page();
    }

    [BindProperty]
    public UserRequestDto AppUser { get; set; } = default!;


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var appUser = new AppUser
        {
            Id = Guid.NewGuid(),
            Active = true,
            Lastname = AppUser.Lastname,
            UserName = AppUser.Username,
            Email = AppUser.Username,
            EmailConfirmed = true,
            Firstname = AppUser.Firstname,
            NormalizedEmail = AppUser.Username.ToUpper(),
            NormalizedUserName = AppUser.Username.ToUpper(),
            PhoneNumberConfirmed = true,
            PositionId = AppUser.PositionId,
            RegisterDate = DateTimeOffset.UtcNow
        };
        await _userManager.CreateAsync(appUser, AppUser.Password);

        return RedirectToPage("./Index");
    }
}