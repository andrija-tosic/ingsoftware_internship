using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business.Services;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.VacationRequests
{
    public class DeleteModel : PageModel
    {
        private readonly IVacationService _vacationService;

        public DeleteModel(IVacationService vacationService)
        {
            _vacationService = vacationService;
        }

        [BindProperty]
        public VacationRequest VacationRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var vacationrequest = await _vacationService.GetVacationRequestByIdAsync((int)id);

            if (vacationrequest is null)
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
            if (id is null)
            {
                return NotFound();
            }

            await _vacationService.DeleteVacationRequestAsync((int)id, User);

            return RedirectToPage("./Index");
        }
    }
}
