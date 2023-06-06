using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.VacationRequests
{
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> OnGet()
        {
            LeaveTypes = await _unitOfWork.VacationService.GetLeaveTypes();

            return RedirectToPage("./Index");
        }

        [BindProperty]
        public VacationRequest VacationRequest { get; set; } = default!;

        public IList<LeaveType> LeaveTypes { get; set; } = default!;
        
        [BindProperty]
        public int LeaveTypeId { get; set; } = default;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            LeaveType? leaveType = await _unitOfWork.VacationService.GetLeaveTypeById(LeaveTypeId);

            if (leaveType is null)
            {
                return NotFound();
            }

            VacationRequest.LeaveType = leaveType;

            Employee? loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

            if (loggedInEmployee is null)
            {
                return NotFound();
            }

            VacationRequest.Employee = loggedInEmployee;
            _unitOfWork.VacationService.CreateVacationRequest(VacationRequest);

            await _unitOfWork.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
