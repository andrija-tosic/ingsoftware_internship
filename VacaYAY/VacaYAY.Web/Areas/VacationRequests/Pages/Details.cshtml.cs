using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.VacationRequests;

public class DetailsModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public DetailsModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

  public VacationRequest VacationRequest { get; set; } = default!; 

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var vacationRequest = await _unitOfWork.VacationService.GetVacationRequestByIdAsync((int)id);

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
