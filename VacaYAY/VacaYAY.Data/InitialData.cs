using Microsoft.AspNetCore.Identity;
using System.Text;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Data;

public static class InitialData
{
    public static Position AdminPosition { get; } = new Position
    {
        Id = 1,
        Caption = "HR",
        Description = "Human Resources",
        Employees = new List<Employee>()
    };

    public static Position[] Positions { get; } = new Position[]
    {
        AdminPosition,
        new Position
        {
            Id = 2,
            Caption = "iOS Developer",
            Description = "Apple user",
            Employees = new List<Employee>()
        },
        new Position
        {
            Id = 3,
            Caption = "Android Developer",
            Description = "Android user",
            Employees = new List<Employee>()
        },
        new Position
        {
            Id = 4,
            Caption = "MVC Intern",
            Description = "Lizard",
            Employees = new List<Employee>()
        },
        new Position
        {
            Id = 5,
            Caption = "Java Intern",
            Description = "Also lizard",
            Employees = new List<Employee>()
        }
    };
    public static EmployeeForDbSeeding AdminEmployee { get; } = new EmployeeForDbSeeding
    {
        Id = new StringBuilder(8 + 1 + 4 + 1 + 4 + 1 + 4 + 1 + 12)
                            .Append(string.Concat(Enumerable.Repeat("a", 8)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("a", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("a", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("a", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("a", 12)))
                            .ToString(),
        UserName = "admin@outlook.com",
        NormalizedUserName = "admin@outlook.com".Normalize(),
        Email = "admin@outlook.com",
        NormalizedEmail = "admin@outlook.com".Normalize(),
        Address = "Vodo Elektro 13",
        DaysOffNumber = 20,
        EmploymentStartDate = DateTime.Now.Date,
        EmploymentEndDate = DateTime.Now.Date.AddYears(1),
        FirstName = "Administrator",
        LastName = "Outlook",
        IdNumber = "12345",
        InsertDate = DateTime.Now,
        PositionId = AdminPosition.Id
    };

    public static EmployeeForDbSeeding[] Employees { get; } = new EmployeeForDbSeeding[]
    {
        AdminEmployee,
        new EmployeeForDbSeeding
        {
            Id = new StringBuilder(8 + 1 + 4 + 1 + 4 + 1 + 4 + 1 + 12)
                            .Append(string.Concat(Enumerable.Repeat("1", 8)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("1", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("1", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("1", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("1", 12)))
                            .ToString(),
            UserName = "andrija@gmail.com",
            NormalizedUserName = "andrija@gmail.com".Normalize(),
            Email = "andrija@gmail.com",
            NormalizedEmail = "andrija@gmail.com".Normalize(),
            Address = "Svetog Patrijarlimpija 12",
            DaysOffNumber = 20,
            EmploymentStartDate = DateTime.Now.Date,
            EmploymentEndDate = DateTime.Now.Date.AddYears(1),
            FirstName = "Andrija",
            LastName = "Tošić",
            IdNumber = "10000",
            InsertDate = DateTime.Now,
            PositionId = Positions[3].Id
        },
        new EmployeeForDbSeeding
        {
            Id = new StringBuilder(8 + 1 + 4 + 1 + 4 + 1 + 4 + 1 + 12)
                            .Append(string.Concat(Enumerable.Repeat("2", 8)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("2", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("2", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("2", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("2", 12)))
                            .ToString(),
            UserName = "papak.potočar@gmail.com",
            NormalizedUserName = "papak.potočar@gmail.com".Normalize(),
            Email = "papak.potočar@gmail.com",
            NormalizedEmail = "papak.potočar@gmail.com".Normalize(),
            Address = "Dino Mustafić 8",
            DaysOffNumber = 20,
            EmploymentStartDate = DateTime.Now.Date,
            EmploymentEndDate = DateTime.Now.Date.AddYears(1),
            FirstName = "Papak",
            LastName = "Potočar",
            IdNumber = "10001",
            InsertDate = DateTime.Now,
            PositionId = Positions[4].Id
        },
        new EmployeeForDbSeeding
        {
            Id = new StringBuilder(8 + 1 + 4 + 1 + 4 + 1 + 4 + 1 + 12)
                            .Append(string.Concat(Enumerable.Repeat("3", 8)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("3", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("3", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("3", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("3", 12)))
                            .ToString(),
            UserName = "katrafilov.f@gmail.com",
            NormalizedUserName = "katrafilov.f@gmail.com".Normalize(),
            Email = "katrafilov.f@gmail.com",
            NormalizedEmail = "katrafilov.f@gmail.com".Normalize(),
            PasswordHash = new PasswordHasher<Employee>().HashPassword(default!, "password"),
            SecurityStamp = string.Empty,
            AccessFailedCount = 0,
            LockoutEnabled = true,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            Address = "S.T.R. Gugleta",
            DaysOffNumber = 20,
            EmploymentStartDate = DateTime.Now.Date,
            EmploymentEndDate = DateTime.Now.Date.AddYears(1),
            FirstName = "Katrafilov",
            LastName = "F",
            IdNumber = "10002",
            InsertDate = DateTime.Now,
            PositionId = Positions[2].Id
        },
        new EmployeeForDbSeeding
        {
            Id = new StringBuilder(8 + 1 + 4 + 1 + 4 + 1 + 4 + 1 + 12)
                            .Append(string.Concat(Enumerable.Repeat("4", 8)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("4", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("4", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("4", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("4", 12)))
                            .ToString(),
            UserName = "jagan.drankovic@gmail.com",
            NormalizedUserName = "jagan.drankovic@gmail.com".Normalize(),
            Email = "jagan.drankovic@gmail.com",
            NormalizedEmail = "jagan.drankovic@gmail.com".Normalize(),
            PasswordHash = new PasswordHasher<Employee>().HashPassword(default!, "password"),
            SecurityStamp = string.Empty,
            AccessFailedCount = 0,
            LockoutEnabled = true,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            Address = "Željka Radeljića Škoda Roomster",
            DaysOffNumber = 20,
            EmploymentStartDate = DateTime.Now.Date,
            EmploymentEndDate = DateTime.Now.Date.AddYears(1),
            FirstName = "Jagan",
            LastName = "Dranković",
            IdNumber = "10003",
            InsertDate = DateTime.Now,
            PositionId = Positions[1].Id,
        },
        new EmployeeForDbSeeding
        {
            Id = new StringBuilder(8 + 1 + 4 + 1 + 4 + 1 + 4 + 1 + 12)
                            .Append(string.Concat(Enumerable.Repeat("5", 8)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("5", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("5", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("5", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("5", 12)))
                            .ToString(),
            UserName = "menza.projic@gmail.com",
            NormalizedUserName = "menza.projic@gmail.com".Normalize(),
            Email = "menza.projic@gmail.com",
            NormalizedEmail = "menza.projic@gmail.com".Normalize(),
            PasswordHash = new PasswordHasher<Employee>().HashPassword(default!, "password"),
            SecurityStamp = string.Empty,
            AccessFailedCount = 0,
            LockoutEnabled = true,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            Address = "Derek Kentford Ave",
            DaysOffNumber = 20,
            EmploymentStartDate = DateTime.Now.Date,
            EmploymentEndDate = DateTime.Now.Date.AddYears(1),
            FirstName = "Menza",
            LastName = "Projić",
            IdNumber = "10004",
            InsertDate = DateTime.Now,
            PositionId = Positions[4].Id,
        },
        new EmployeeForDbSeeding
        {
            Id = new StringBuilder(8 + 1 + 4 + 1 + 4 + 1 + 4 + 1 + 12)
                            .Append(string.Concat(Enumerable.Repeat("6", 8)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("6", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("6", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("6", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("6", 12)))
                            .ToString(),
            UserName = "goran.los.andjeles@gmail.com",
            NormalizedUserName = "goran.los.andjeles@gmail.com".Normalize(),
            Email = "goran.los.andjeles@gmail.com",
            NormalizedEmail = "goran.los.andjeles@gmail.com".Normalize(),
            PasswordHash = new PasswordHasher<Employee>().HashPassword(default!, "password"),
            SecurityStamp = string.Empty,
            AccessFailedCount = 0,
            LockoutEnabled = true,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            Address = "Dylan McKenzie St.",
            DaysOffNumber = 20,
            EmploymentStartDate = DateTime.Now.Date,
            EmploymentEndDate = DateTime.Now.Date.AddYears(1),
            FirstName = "Goran",
            LastName = "Los Anđeles",
            IdNumber = "10005",
            InsertDate = DateTime.Now,
            PositionId = Positions[3].Id,
        },
        new EmployeeForDbSeeding
        {
            Id = new StringBuilder(8 + 1 + 4 + 1 + 4 + 1 + 4 + 1 + 12)
                            .Append(string.Concat(Enumerable.Repeat("7", 8)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("7", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("7", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("7", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("7", 12)))
                            .ToString(),
            UserName = "milka.ladovinka@gmail.com",
            NormalizedUserName = "milka.ladovinka@gmail.com".Normalize(),
            Email = "milka.ladovinka@gmail.com",
            NormalizedEmail = "milka.ladovinka@gmail.com".Normalize(),
            PasswordHash = new PasswordHasher<Employee>().HashPassword(default!, "password"),
            SecurityStamp = string.Empty,
            AccessFailedCount = 0,
            LockoutEnabled = true,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            Address = "Sokače \"Sv. Trifutin\"",
            DaysOffNumber = 20,
            EmploymentStartDate = DateTime.Now.Date,
            EmploymentEndDate = DateTime.Now.Date.AddYears(1),
            FirstName = "Milka",
            LastName = "Ladovinka",
            IdNumber = "10006",
            InsertDate = DateTime.Now,
            PositionId = Positions[1].Id,
        },
        new EmployeeForDbSeeding
        {
            Id = new StringBuilder(8 + 1 + 4 + 1 + 4 + 1 + 4 + 1 + 12)
                            .Append(string.Concat(Enumerable.Repeat("8", 8)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("8", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("8", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("8", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("8", 12)))
                            .ToString(),
            UserName = "mustafa.hrustic@gmail.com",
            NormalizedUserName = "mustafa.hrustic@gmail.com".Normalize(),
            Email = "mustafa.hrustic@gmail.com",
            NormalizedEmail = "mustafa.hrustic@gmail.com".Normalize(),
            PasswordHash = new PasswordHasher<Employee>().HashPassword(default!, "password"),
            SecurityStamp = string.Empty,
            AccessFailedCount = 0,
            LockoutEnabled = true,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            Address = "Ispod mosta, Zenica",
            DaysOffNumber = 20,
            EmploymentStartDate = DateTime.Now.Date,
            EmploymentEndDate = DateTime.Now.Date.AddYears(1),
            FirstName = "Mustafa",
            LastName = "Hrustić",
            IdNumber = "10007",
            InsertDate = DateTime.Now,
            PositionId = Positions[2].Id,
        },
        new EmployeeForDbSeeding
        {
            Id = new StringBuilder(8 + 1 + 4 + 1 + 4 + 1 + 4 + 1 + 12)
                            .Append(string.Concat(Enumerable.Repeat("9", 8)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("9", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("9", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("9", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("9", 12)))
                            .ToString(),
            UserName = "boban.gasev@gmail.com",
            NormalizedUserName = "boban.gasev@gmail.com".Normalize(),
            Email = "boban.gasev@gmail.com",
            NormalizedEmail = "boban.gasev@gmail.com".Normalize(),
            PasswordHash = new PasswordHasher<Employee>().HashPassword(default!, "password"),
            SecurityStamp = string.Empty,
            AccessFailedCount = 0,
            LockoutEnabled = true,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            Address = "Bogoljuba Bradostanojevića",
            DaysOffNumber = 20,
            EmploymentStartDate = DateTime.Now.Date,
            EmploymentEndDate = DateTime.Now.Date.AddYears(1),
            FirstName = "Boban",
            LastName = "Gasev",
            IdNumber = "10008",
            InsertDate = DateTime.Now,
            PositionId = Positions[3].Id,
        },
        new EmployeeForDbSeeding
        {
            Id = new StringBuilder(8 + 1 + 4 + 1 + 4 + 1 + 4 + 1 + 12)
                            .Append(string.Concat(Enumerable.Repeat("e", 8)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("e", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("e", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("e", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("e", 12)))
                            .ToString(),
            UserName = "erl.znojsulja@gmail.com",
            NormalizedUserName = "erl.znojsulja@gmail.com".Normalize(),
            Email = "erl.znojsulja@gmail.com",
            NormalizedEmail = "erl.znojsulja@gmail.com".Normalize(),
            PasswordHash = new PasswordHasher<Employee>().HashPassword(default!, "password"),
            SecurityStamp = string.Empty,
            AccessFailedCount = 0,
            LockoutEnabled = true,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            Address = "Patrijarha Veropojlija",
            DaysOffNumber = 20,
            EmploymentStartDate = DateTime.Now.Date,
            EmploymentEndDate = DateTime.Now.Date.AddYears(1),
            FirstName = "Erl",
            LastName = "Znojšulja",
            IdNumber = "10009",
            InsertDate = DateTime.Now,
            PositionId = Positions[3].Id,
        },
    };

    public const string AdminRoleName = "Administrator";
    public const string DefaultRoleName = "Default";
    public static IdentityRole AdminIdentityRole { get; } = new IdentityRole
    {
        Id = new StringBuilder(8 + 1 + 4 + 1 + 4 + 1 + 4 + 1 + 12)
                            .Append(string.Concat(Enumerable.Repeat("a", 8)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("a", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("a", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("a", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("a", 12)))
                            .ToString(),
        Name = AdminRoleName,
        NormalizedName = AdminRoleName.Normalize()
    };
    public static IdentityRole DefaultIdentityRole { get; } = new IdentityRole
    {
        Id = new StringBuilder(8 + 1 + 4 + 1 + 4 + 1 + 4 + 1 + 12)
                            .Append(string.Concat(Enumerable.Repeat("d", 8)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("d", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("d", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("d", 4)))
                            .Append("-")
                            .Append(string.Concat(Enumerable.Repeat("d", 12)))
                            .ToString(),
        Name = DefaultRoleName,
        NormalizedName = DefaultRoleName.Normalize()
    };

    public static IdentityRole[] IdentityRoles { get; } = new IdentityRole[] { AdminIdentityRole, DefaultIdentityRole };

    public static LeaveType[] LeaveTypes { get; } = new LeaveType[]
    {
        new LeaveType
        {
            Id = 1,
            Name = "Days off",
            VacationRequests = new List<VacationRequest>()
        },
        new LeaveType
        {
            Id = 2,
            Name = "Sick leave",
            VacationRequests = new List<VacationRequest>()
        },
        new LeaveType
        {
            Id = 3,
            Name = "Paid leave",
            VacationRequests = new List<VacationRequest>()
        },
        new LeaveType
        {
            Id = 4,
            Name = "Unpaid leave",
            VacationRequests = new List<VacationRequest>()
        }
    };
}
