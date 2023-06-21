// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business;
using VacaYAY.Data;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.Identity.Pages.Account;

[Authorize(Roles = InitialData.AdminRoleName)]
public class RegisterModel : PageModel
{
    private readonly SignInManager<Employee> _signInManager;
    private readonly UserManager<Employee> _userManager;
    private readonly ILogger<RegisterModel> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public IEnumerable<Position> Positions { get; set; }

    public RegisterModel(
        UserManager<Employee> userManager,
        SignInManager<Employee> signInManager,
        ILogger<RegisterModel> logger,
        IUnitOfWork unitOfWork
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public class InputModel
    {
        [Required]
        [Display(Name = "First name")]
        public required string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = nameof(Address))]
        public required string Address { get; set; }

        [Required]
        [Display(Name = "ID number")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "ID number must contain numbers only.")]
        public required string IdNumber { get; set; }

        [Required]
        [Display(Name = "Days off")]
        public required int DaysOffNumber { get; set; }

        [Required]
        [Display(Name = nameof(Position))]
        public required int PositionId { get; set; }

        [Required]
        [Display(Name = "Employment start date")]
        public required DateTime EmploymentStartDate { get; set; }
        [Display(Name = "Employment end date")]
        public DateTime? EmploymentEndDate { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
        ReturnUrl = returnUrl;
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        Positions = await _unitOfWork.PositionService.GetAllAsync();
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        Positions = await _unitOfWork.PositionService.GetAllAsync();
        
        returnUrl ??= Url.Content("~/" + nameof(Employees));
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        if (ModelState.IsValid)
        {
            var user = new Employee
            {
                Email = Input.Email,
                Address = Input.Address,
                DaysOffNumber = Input.DaysOffNumber,
                EmploymentStartDate = Input.EmploymentStartDate,
                EmploymentEndDate = Input.EmploymentEndDate,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                IdNumber = Input.IdNumber,
                InsertDate = DateTime.Now.Date,
                Position = await _unitOfWork.PositionService.GetByIdAsync(Input.PositionId),
                VacationRequests = new List<VacationRequest>(),
                VacationReviews = new List<VacationReview>(),
                Contracts = new List<Contract>()
            };

            var validationResult = await _unitOfWork.EmployeeService.CreateAsync(user, Input.Password);

            if (validationResult.IsValid)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("User created a new account with password.");

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                }
                else
                {
                    return LocalRedirect(returnUrl);
                }
            }
            ModelState.Clear();
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}
