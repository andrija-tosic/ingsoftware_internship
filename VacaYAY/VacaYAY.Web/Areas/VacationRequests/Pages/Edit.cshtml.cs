using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VacaYAY.Business;
using VacaYAY.Data;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.VacationRequests
{
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public VacationRequest VacationRequest { get; set; } = default!;

        public IList<LeaveType> LeaveTypes { get; set; } = default!;

        [BindProperty]
        public int LeaveTypeId { get; set; } = default;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            LeaveTypes = await _unitOfWork.VacationService.GetLeaveTypes();

            var vacationRequest = await _unitOfWork.VacationService.GetVacationRequestByIdAsync((int)id);
            if (vacationRequest == null)
            {
                return NotFound();
            }

            LeaveTypeId = vacationRequest.LeaveType.Id;
            VacationRequest = vacationRequest;
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var leaveType = await _unitOfWork.VacationService.GetLeaveTypeById(LeaveTypeId);

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

            _unitOfWork.VacationService.UpdateVacationRequest(VacationRequest);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return RedirectToPage("./Index");
        }
    }
}
