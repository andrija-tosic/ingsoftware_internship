using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Data;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.VacationRequests
{
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public VacationRequest VacationRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var vacationrequest = await _unitOfWork.VacationService.GetVacationRequestByIdAsync((int)id);

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

            var loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

            if (loggedInEmployee is null)
            {
                return Unauthorized();
            }

            var vacationrequest = await _unitOfWork.VacationService.GetVacationRequestByIdAsync((int)id);

            if (vacationrequest is null)
            {
                return NotFound();
            }

            VacationRequest = vacationrequest;
            await _unitOfWork.VacationService.DeleteVacationRequestAsync(VacationRequest.Id);

            int days = (VacationRequest.EndDate - VacationRequest.StartDate).Days;
            VacationRequest.Employee.DaysOffNumber += days;
            await _unitOfWork.EmployeeService.UpdateAsync(VacationRequest.Employee);

            await _unitOfWork.SaveChangesAsync();

            var hrEmployees = await _unitOfWork.EmployeeService.GetByPositions(new[] { InitialData.AdminPosition.Id });

            string emailSubject = $"Vacation request from {VacationRequest.Employee.FirstName} {VacationRequest.Employee.LastName} deleted";
            string emailBody = VacationRequest.ToString();

            foreach (var e in hrEmployees)
            {
                _unitOfWork.EmailService.EnqueueEmail(e.Email!, emailSubject, emailBody);
            }

            _unitOfWork.EmailService.EnqueueEmail(loggedInEmployee.Email!, emailSubject, emailBody);

            return RedirectToPage("./Index");
        }
    }
}
