using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business.Services;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.VacationRequests;

public class DetailsModel : PageModel
{
    private readonly IVacationService _vacationService;

    public DetailsModel(IVacationService vacationService)
    {
        _vacationService = vacationService;
    }

  public VacationRequest VacationRequest { get; set; } = default!; 

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var vacationRequest = await _vacationService.GetVacationRequestByIdAsync((int)id);

        if (vacationRequest is null)
        {
            return NotFound();
        }
        else 
        {
            VacationRequest = vacationRequest;
        }
        return Page();
    }
}
