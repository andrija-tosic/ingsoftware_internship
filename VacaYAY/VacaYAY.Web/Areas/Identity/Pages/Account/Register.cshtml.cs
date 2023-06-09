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

[Authorize(Roles = nameof(UserRoles.Administrator))]
public class RegisterModel : PageModel
{
    private readonly SignInManager<Employee> _signInManager;
    private readonly UserManager<Employee> _userManager;
    private readonly IUserStore<Employee> _userStore;
    //private readonly IUserEmailStore<Employee> _emailStore;
    private readonly ILogger<RegisterModel> _logger;
    //private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    //private readonly IEmailSender _emailSender;

    public IEnumerable<Position> Positions { get; set; }

    public RegisterModel(
        UserManager<Employee> userManager,
        IUserStore<Employee> userStore,
        SignInManager<Employee> signInManager,
        ILogger<RegisterModel> logger,
        //,IEmailSender emailSender
        //RoleManager<IdentityRole> roleManager,
        IUnitOfWork unitOfWork
        )
    {
        _userManager = userManager;
        _userStore = userStore;
        //_emailStore = GetEmailStore();
        _signInManager = signInManager;
        _logger = logger;
        //_roleManager = roleManager;
        _unitOfWork = unitOfWork;

        //_emailSender = emailSender;
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
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
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
        [Required]
        [Display(Name = "Employment end date")]
        public required DateTime EmploymentEndDate { get; set; }

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
        
        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        if (ModelState.IsValid)
        {
            Employee user = await CreateUser();
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
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
            }
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

    private async Task<Employee> CreateUser()
    {
        return new Employee
        {
            Email = Input.Email,
            Address = Input.Address,
            DaysOffNumber = Input.DaysOffNumber,
            EmploymentStartDate = Input.EmploymentStartDate,
            EmploymentEndDate = Input.EmploymentEndDate,
            FirstName = Input.FirstName,
            LastName = Input.LastName,
            IdNumber = Input.IdNumber,
            InsertDate = DateTime.Now,
            Position = await _unitOfWork.PositionService.GetByIdAsync(Input.PositionId),
            VacationRequests = new List<VacationRequest>(),
            VacationReviews = new List<VacationReview>()
        };
    }

    private IUserEmailStore<Employee> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<Employee>)_userStore;
    }
}
