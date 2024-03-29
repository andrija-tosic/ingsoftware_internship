﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business.Services;
using VacaYAY.Business.Validators;
using VacaYAY.Data;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Web.Areas.Identity.Pages.Account;

[Authorize(Roles = InitialData.AdminRoleName)]
public class RegisterModel : PageModel
{
    private readonly SignInManager<Employee> _signInManager;
    private readonly UserManager<Employee> _userManager;
    private readonly ILogger<RegisterModel> _logger;
    private readonly IEmployeeService _employeeService;
    private readonly IPositionService _positionService;
    private readonly IContractService _contractService;
    private readonly IFileService _fileService;

    public IEnumerable<Position> Positions { get; set; }
    public IList<ContractType> ContractTypes { get; set; }

    public RegisterModel(
        UserManager<Employee> userManager,
        SignInManager<Employee> signInManager,
        ILogger<RegisterModel> logger,
        IEmployeeService employeeService,
        IPositionService positionService,
        IContractService contractService,
        IFileService fileService
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _employeeService = employeeService;
        _positionService = positionService;
        _contractService = contractService;
        _fileService = fileService;
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
        [DisplayName("Employment start date")]
        public required DateTime EmploymentStartDate { get; set; }
        [DisplayName("Employment end date")]
        public DateTime? EmploymentEndDate { get; set; }
        public required ContractDTO ContractDTO { get; set; }

        [Required(ErrorMessage = "Contract document is required.")]
        public required IFormFile ContractFile { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
        ReturnUrl = returnUrl;
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        Positions = await _positionService.GetAllAsync();
        ContractTypes = await _contractService.GetContractTypesAsync();
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        Positions = await _positionService.GetAllAsync();
        ContractTypes = await _contractService.GetContractTypesAsync();

        Uri contractFileUrl = await _fileService.SaveFileAsync(Input.ContractFile);

        var contract = new Contract()
        {
            Employee = null,
            Number = Input.ContractDTO.Number,
            StartDate = Input.ContractDTO.StartDate,
            EndDate = Input.ContractDTO.EndDate,
            Type = ContractTypes.Single(ct => ct.Id == Input.ContractDTO.ContractTypeId),
            DocumentUrl = contractFileUrl.ToString()
        };

        var contractValidationResult = new ContractValidator().Validate(contract);

        if (!contractValidationResult.IsValid)
        {
            ModelState.Clear();
            foreach (var error in contractValidationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return Page();
        }

        returnUrl ??= Url.Content("~/" + nameof(Employees));
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        var user = new EmployeeDTO
        {
            Id = string.Empty,
            Email = Input.Email,
            Address = Input.Address,
            DaysOffNumber = Input.DaysOffNumber,
            LastYearsDaysOffNumber = 0,
            EmploymentStartDate = Input.EmploymentStartDate,
            EmploymentEndDate = Input.EmploymentEndDate,
            FirstName = Input.FirstName,
            LastName = Input.LastName,
            IdNumber = Input.IdNumber,
            InsertDate = DateTime.Now.Date,
            PositionId = Input.PositionId,
            Contract = contract
        };

        var employeeValidationResult = await _employeeService.CreateAsync(user, Input.Password);

        if (employeeValidationResult.IsValid)
        {
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
        else
        {
            ModelState.Clear();
            foreach (var error in employeeValidationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return Page();
        }
    }
}
