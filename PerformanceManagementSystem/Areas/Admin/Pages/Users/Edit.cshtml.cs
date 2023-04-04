using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views;
using PerformanceManagementSystem.Data.Views.Positions;
using PerformanceManagementSystem.Data.Views.Users;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Users;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public EditUserRequestDto AppUser { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null || _context.Users == null)
        {
            return NotFound();
        }

        var appuser = await _context.Users
            .Include(a => a.Managers).FirstOrDefaultAsync(m => m.Id == id);
        if (appuser == null)
        {
            return NotFound();
        }
        ViewData["Managers"] = new SelectList(_context.Users.Where(a => a.Id != appuser.Id).Select(a => new UserResponseDto
        {
            Id = a.Id,
            Name = $"{a.Firstname} {a.Lastname} ({a.Email})"
        }), "Id", "Name", appuser.Managers.Select(a => a.ManagerId).ToList());

        ViewData["PositionId"] = new SelectList(_context.Positions.Where(a => a.Active && !a.Deleted).Select(a => new PositionResponseDto
        {
            Title = a.Title,
            Id = a.Id
        }), "Id", "Title", appuser.PositionId);

        AppUser = new EditUserRequestDto
        {
            Firstname = appuser.Firstname,
            Id = appuser.Id,
            Lastname = appuser.Lastname,
            PositionId = appuser.PositionId,
            ManagerIds = appuser.Managers.Select(a => a.ManagerId).ToList()
        };
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var appUser = await _context.Users.FirstOrDefaultAsync(a => a.Id == AppUser.Id);

        if (appUser == null)
            return RedirectToPage("./Index");

        appUser.Managers = AppUser.ManagerIds.Select(b => new UserManagerMapping
        {
            ManagerId = b,
            UserId = appUser.Id
        }).ToList();

        appUser.Firstname = AppUser.Firstname;
        appUser.Lastname = AppUser.Lastname;
        appUser.PositionId = AppUser.PositionId;

        _context.Users.Update(appUser);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AppUserExists(AppUser.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool AppUserExists(Guid id)
    {
        return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}