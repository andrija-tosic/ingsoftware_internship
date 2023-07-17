using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business.Services;
using VacaYAY.Data;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.Contracts.Pages;

[Authorize(Roles = InitialData.AdminRoleName)]
public class IndexModel : PageModel
{
    private readonly IContractService _contractService;

    public IndexModel(IContractService contractService)
    {
        _contractService = contractService;
    }

    public IList<Contract> Contracts { get;set; } = default!;

    public async Task OnGetAsync()
    {
        Contracts = await _contractService.GetAllAsync();
    }
}
