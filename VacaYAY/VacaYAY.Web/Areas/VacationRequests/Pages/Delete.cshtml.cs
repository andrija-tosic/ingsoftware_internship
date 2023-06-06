using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VacaYAY.Data;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.VacationRequests
{
    public class DeleteModel : PageModel
    {
        private readonly VacayayDbContext _context;

        public DeleteModel(VacayayDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public VacationRequest VacationRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.VacationRequests == null)
            {
                return NotFound();
            }

            var vacationrequest = await _context.VacationRequests.FirstOrDefaultAsync(m => m.Id == id);

            if (vacationrequest == null)
            {
                return NotFound();
            }
            else 
            {
                VacationRequest = vacationrequest;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.VacationRequests == null)
            {
                return NotFound();
            }
            var vacationrequest = await _context.VacationRequests.FindAsync(id);

            if (vacationrequest != null)
            {
                VacationRequest = vacationrequest;
                _context.VacationRequests.Remove(VacationRequest);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
