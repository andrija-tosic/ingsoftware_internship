using Microsoft.AspNetCore.Identity;
using VacaYAY.Data.DTOs;
using VacaYAY.Data.Models;

namespace VacaYAY.Data;

public static class InitialData
{
    private static DateTime ExampleDate { get; } = new DateTime(2023, 5, 25).Date;
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
        Id = "000f4869-1f3a-4d63-a92d-6fa8753aa353",
        UserName = "admin@outlook.com",
        NormalizedUserName = "admin@outlook.com".Normalize(),
        Email = "admin@outlook.com",
        NormalizedEmail = "admin@outlook.com".Normalize(),
        Address = "Vodo Elektro 13",
        DaysOffNumber = 20,
        EmploymentStartDate = ExampleDate,
        EmploymentEndDate = ExampleDate.AddYears(1),
        FirstName = "Administrator",
        LastName = "Outlook",
        IdNumber = "12345",
        InsertDate = ExampleDate,
        PositionId = AdminPosition.Id
    };

    public static EmployeeForDbSeeding[] Employees { get; } = new EmployeeForDbSeeding[]
    {
        AdminEmployee,
        new EmployeeForDbSeeding
        {
            Id = "19eadb6f-7ed7-4acd-9bf4-26825f5619a7",
            UserName = "andrija.tosic@ingsoftware.com",
            NormalizedUserName = "andrija.tosic@ingsoftware.com".Normalize(),
            Email = "andrija.tosic@ingsoftware.com",
            NormalizedEmail = "andrija.tosic@ingsoftware.com".Normalize(),
            Address = "Svetog Patrijarlimpija 12",
            DaysOffNumber = 20,
            EmploymentStartDate = ExampleDate,
            EmploymentEndDate = ExampleDate.AddYears(1),
            FirstName = "Andrija",
            LastName = "Tošić",
            IdNumber = "10000",
            InsertDate = ExampleDate,
            PositionId = Positions[3].Id
        },
        new EmployeeForDbSeeding
        {
            Id = "aac218f6-96c4-47c0-b231-310b7f7f6a85",
            UserName = "papak.potočar@gmail.com",
            NormalizedUserName = "papak.potočar@gmail.com".Normalize(),
            Email = "papak.potočar@gmail.com",
            NormalizedEmail = "papak.potočar@gmail.com".Normalize(),
            Address = "Dino Mustafić 8",
            DaysOffNumber = 20,
            EmploymentStartDate = ExampleDate,
            EmploymentEndDate = ExampleDate.AddYears(1),
            FirstName = "Papak",
            LastName = "Potočar",
            IdNumber = "10001",
            InsertDate = ExampleDate,
            PositionId = Positions[4].Id
        },
        new EmployeeForDbSeeding
        {
            Id = "38f4bcd5-eab1-45a7-9929-2fb8550dbe57",
            UserName = "katrafilov.f@gmail.com",
            NormalizedUserName = "katrafilov.f@gmail.com".Normalize(),
            Email = "katrafilov.f@gmail.com",
            NormalizedEmail = "katrafilov.f@gmail.com".Normalize(),
            Address = "S.T.R. Gugleta",
            DaysOffNumber = 20,
            EmploymentStartDate = ExampleDate,
            EmploymentEndDate = ExampleDate.AddYears(1),
            FirstName = "Katrafilov",
            LastName = "F",
            IdNumber = "10002",
            InsertDate = ExampleDate,
            PositionId = Positions[2].Id
        },
        new EmployeeForDbSeeding
        {
            Id = "069fe119-c139-4d6f-a4d5-0bc90540339f",
            UserName = "jagan.drankovic@gmail.com",
            NormalizedUserName = "jagan.drankovic@gmail.com".Normalize(),
            Email = "jagan.drankovic@gmail.com",
            NormalizedEmail = "jagan.drankovic@gmail.com".Normalize(),
            Address = "Željka Radeljića Škoda Roomster",
            DaysOffNumber = 20,
            EmploymentStartDate = ExampleDate,
            EmploymentEndDate = ExampleDate.AddYears(1),
            FirstName = "Jagan",
            LastName = "Dranković",
            IdNumber = "10003",
            InsertDate = ExampleDate,
            PositionId = Positions[1].Id,
        },
        new EmployeeForDbSeeding
        {
            Id = "24105a5d-752a-4f0d-b992-787388f159bf",
            UserName = "menza.projic@gmail.com",
            NormalizedUserName = "menza.projic@gmail.com".Normalize(),
            Email = "menza.projic@gmail.com",
            NormalizedEmail = "menza.projic@gmail.com".Normalize(),
            Address = "Derek Kentford Ave",
            DaysOffNumber = 20,
            EmploymentStartDate = ExampleDate,
            EmploymentEndDate = ExampleDate.AddYears(1),
            FirstName = "Menza",
            LastName = "Projić",
            IdNumber = "10004",
            InsertDate = ExampleDate,
            PositionId = Positions[4].Id,
        },
        new EmployeeForDbSeeding
        {
            Id = "335ef315-05f4-4d87-a678-05ec02de608f",
            UserName = "goran.los.andjeles@gmail.com",
            NormalizedUserName = "goran.los.andjeles@gmail.com".Normalize(),
            Email = "goran.los.andjeles@gmail.com",
            NormalizedEmail = "goran.los.andjeles@gmail.com".Normalize(),
            Address = "Dylan McKenzie St.",
            DaysOffNumber = 20,
            EmploymentStartDate = ExampleDate,
            EmploymentEndDate = ExampleDate.AddYears(1),
            FirstName = "Goran",
            LastName = "Los Anđeles",
            IdNumber = "10005",
            InsertDate = ExampleDate,
            PositionId = Positions[3].Id,
        },
        new EmployeeForDbSeeding
        {
            Id = "cc89ca61-a1bf-42a0-b4b6-a2c548ffb6a6",
            UserName = "milka.ladovinka@gmail.com",
            NormalizedUserName = "milka.ladovinka@gmail.com".Normalize(),
            Email = "milka.ladovinka@gmail.com",
            NormalizedEmail = "milka.ladovinka@gmail.com".Normalize(),
            Address = "Sokače \"Sv. Trifutin\"",
            DaysOffNumber = 20,
            EmploymentStartDate = ExampleDate,
            EmploymentEndDate = ExampleDate.AddYears(1),
            FirstName = "Milka",
            LastName = "Ladovinka",
            IdNumber = "10006",
            InsertDate = ExampleDate,
            PositionId = Positions[1].Id,
        },
        new EmployeeForDbSeeding
        {
            Id = "ca0a5ad3-689c-4915-8261-01a67889e664",
            UserName = "mustafa.hrustic@gmail.com",
            NormalizedUserName = "mustafa.hrustic@gmail.com".Normalize(),
            Email = "mustafa.hrustic@gmail.com",
            NormalizedEmail = "mustafa.hrustic@gmail.com".Normalize(),
            Address = "Ispod mosta, Zenica",
            DaysOffNumber = 20,
            EmploymentStartDate = ExampleDate,
            EmploymentEndDate = ExampleDate.AddYears(1),
            FirstName = "Mustafa",
            LastName = "Hrustić",
            IdNumber = "10007",
            InsertDate = ExampleDate,
            PositionId = Positions[2].Id,
        },
        new EmployeeForDbSeeding
        {
            Id = "3e21a209-9d4c-44e3-8e97-f7ed042c9c56",
            UserName = "boban.gasev@gmail.com",
            NormalizedUserName = "boban.gasev@gmail.com".Normalize(),
            Email = "boban.gasev@gmail.com",
            NormalizedEmail = "boban.gasev@gmail.com".Normalize(),
            Address = "Bogoljuba Bradostanojevića",
            DaysOffNumber = 20,
            EmploymentStartDate = ExampleDate,
            EmploymentEndDate = ExampleDate.AddYears(1),
            FirstName = "Boban",
            LastName = "Gasev",
            IdNumber = "10008",
            InsertDate = ExampleDate,
            PositionId = Positions[3].Id,
        },
        new EmployeeForDbSeeding
        {
            Id = "2f409517-b274-44ea-9380-c57fff02871d",
            UserName = "erl.znojsulja@gmail.com",
            NormalizedUserName = "erl.znojsulja@gmail.com".Normalize(),
            Email = "erl.znojsulja@gmail.com",
            NormalizedEmail = "erl.znojsulja@gmail.com".Normalize(),
            Address = "Patrijarha Veropojlija",
            DaysOffNumber = 20,
            EmploymentStartDate = ExampleDate,
            EmploymentEndDate = ExampleDate.AddYears(1),
            FirstName = "Erl",
            LastName = "Znojšulja",
            IdNumber = "10009",
            InsertDate = ExampleDate,
            PositionId = Positions[3].Id,
        },
    };

    public const string AdminRoleName = "Administrator";
    public const string DefaultRoleName = "Default";
    public static IdentityRole AdminIdentityRole { get; } = new IdentityRole
    {
        Id = "a94fb548-dc92-4f3f-872b-3bca02114ea8",
        Name = AdminRoleName,
        NormalizedName = AdminRoleName.Normalize()
    };
    public static IdentityRole DefaultIdentityRole { get; } = new IdentityRole
    {
        Id = "76bc4d5c-00ea-48bd-a9b4-fef0d30e4e23",
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
