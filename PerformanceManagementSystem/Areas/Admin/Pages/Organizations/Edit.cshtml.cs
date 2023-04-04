using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;
using PerformanceManagementSystem.Data.Views;
using PerformanceManagementSystem.Data.Views.Organizations;

namespace PerformanceManagementSystem.Areas.Admin.Pages.Organizations
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EditModel(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public OrganizationRequestDto Organization { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations.Select(a => new OrganizationRequestDto
            {
                Id = a.Id,
                Active = a.Active,
                AdminUserId = a.AdminUserId,
                Title = a.Title
            }).FirstOrDefaultAsync(m => m.Id == id);

            if (organization == null)
            {
                return NotFound();
            }
            Organization = organization;
            ViewData["AdminUserId"] = new SelectList(_context.Users.Select(a => new UserResponseDto
            {
                Email = a.Email,
                Id = a.Id,
                Name = $"{a.Firstname} {a.Lastname}"
            }), "Id", "Name");
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

            var organization = _mapper.Map<OrganizationRequestDto, Organization>(Organization);
            organization.UpdatedDate = DateTime.UtcNow;
            _context.Organizations.Update(organization);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationExists(Organization.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToPage("./Index");
        }

        private bool OrganizationExists(Guid id)
        {
            return (_context.Organizations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
