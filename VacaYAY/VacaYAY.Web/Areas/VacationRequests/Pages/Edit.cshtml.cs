using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VacaYAY.Business;
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

            VacationRequest = vacationRequest;

            VacationRequest.VacationReview ??= new VacationRequestReview()
            {
                Approved = false,
                Comment = string.Empty,
                Reviewer = default!,
                VacationRequest = default!
            };

            return Page();
        }
        public async Task<IActionResult> OnPostUpsertVacationRequestReviewAsync()
        {
            Employee? loggedInEmployee = await _unitOfWork.EmployeeService.GetLoggedInAsync(User);

            if (loggedInEmployee is null)
            {
                return NotFound();
            }

            VacationRequest.VacationReview!.VacationRequestRefId = VacationRequest.Id;
            VacationRequest.VacationReview!.Reviewer = loggedInEmployee;

            if (VacationRequest.VacationReview.Id == 0)
            {
                _unitOfWork.VacationService.CreateVacationRequestReview(VacationRequest.VacationReview!);
            }
            else
            {
                _unitOfWork.VacationService.UpdateVacationRequestReview(VacationRequest.VacationReview!);
            }
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDeleteVacationRequestReviewAsync()
        {
            await _unitOfWork.VacationService.DeleteVacationRequestReviewAsync(VacationRequest.VacationReview!.Id);
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
