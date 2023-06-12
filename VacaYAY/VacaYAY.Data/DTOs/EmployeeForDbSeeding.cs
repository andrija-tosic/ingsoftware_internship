﻿using Microsoft.AspNetCore.Identity;
using VacaYAY.Data.Models;

namespace VacaYAY.Data.DTOs;

public class EmployeeForDbSeeding
{
    public EmployeeForDbSeeding()
    {
        EmailConfirmed = true;
        PasswordHash = new PasswordHasher<Employee>().HashPassword(default!, "password");
        SecurityStamp = string.Empty;
        AccessFailedCount = 0;
        LockoutEnabled = true;
        PhoneNumberConfirmed = false;
        TwoFactorEnabled = false;
    }
    public required string Id { get; init; }
    public required string UserName { get; init; }
    public required string NormalizedUserName { get; init; }
    public required string Email { get; init; }
    public required string NormalizedEmail { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required int PositionId { get; init; }
    public required string Address { get; init; }
    public required string IdNumber { get; init; }
    public required int DaysOffNumber { get; init; }
    public required DateTime EmploymentStartDate { get; init; }
    public required DateTime? EmploymentEndDate { get; init; }
    public required DateTime InsertDate { get; init; }
    public bool EmailConfirmed { get; init; }
    public string PasswordHash { get; init; }
    public string SecurityStamp { get; init; }
    public int AccessFailedCount { get; init; }
    public bool LockoutEnabled { get; init; }
    public bool PhoneNumberConfirmed { get; init; }
    public bool TwoFactorEnabled { get; init; }
}