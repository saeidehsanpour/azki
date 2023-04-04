using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views;
using PerformanceManagementSystem.Data.Views.Organizations;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Organizations;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CreateModel(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult OnGet()
    {
        ViewData["AdminUserId"] = new SelectList(_context.Users.Select(a => new UserResponseDto
        {
            Email = a.Email,
            Id = a.Id,
            Name = $"{a.Firstname} {a.Lastname}"
        }), "Id", "Name");
        return Page();
    }

    [BindProperty]
    public OrganizationRequestDto Organization { get; set; } = default!;


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var organization = _mapper.Map<OrganizationRequestDto, Organization>(Organization);
        _context.Organizations.Add(organization);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}