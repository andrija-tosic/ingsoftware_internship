using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Business.Services;
using VacaYAY.Data;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.Contracts.Pages;


[Authorize(Roles = InitialData.AdminRoleName)]
public class DeleteModel : PageModel
{
    private readonly IContractService _contractService;

    public DeleteModel(IContractService contractService)
    {
        _contractService = contractService;
    }

    [BindProperty]
  public Contract Contract { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var contract = await _contractService.GetByIdAsync((int)id);

        if (contract is null)
        {
            return NotFound();
        }
        else 
        {
            Contract = contract;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        var contract = await _contractService.GetByIdAsync((int)id);

        if (contract is not null)
        {
            Contract = contract;
            await _contractService.DeleteContractAsync((int)id);
        }

        return RedirectToPage("./Index");
    }
}
