using Microsoft.AspNetCore.Identity;
using System.Text;
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
        new Position {
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

    public const string AdminRoleName = "Administrator";
    public const string DefaultRoleName = "Default";
    public static IdentityRole AdminRole { get; } = new IdentityRole
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
    public static IdentityRole DefaultRole { get; } = new IdentityRole
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

    public static IdentityRole[] IdentityRoles { get; } = new IdentityRole[] { AdminRole, DefaultRole };
}
