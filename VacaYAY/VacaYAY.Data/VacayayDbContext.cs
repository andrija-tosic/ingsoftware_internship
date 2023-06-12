using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text;
using VacaYAY.Data.Models;

namespace VacaYAY.Data;

public class VacayayDbContext : IdentityDbContext<Employee>
{
    public required DbSet<Employee> Employees { get; set; }
    public required DbSet<Position> Positions { get; set; }
    public required DbSet<VacationRequest> VacationRequests { get; set; }
    public required DbSet<VacationReview> VacationReviews { get; set; }
    public required DbSet<LeaveType> LeaveTypes { get; set; }
    public VacayayDbContext(DbContextOptions<VacayayDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>().ToTable(nameof(Employee) + "s");
        modelBuilder.Entity<Employee>().HasQueryFilter(e => e.DeleteDate == null);
        modelBuilder.Entity<VacationRequest>().HasQueryFilter(r => r.Employee.DeleteDate == null);
        modelBuilder.Entity<VacationReview>().HasQueryFilter(r => r.VacationRequest.Employee.DeleteDate == null);

        modelBuilder.Entity<VacationRequest>()
            .HasOne(r => r.VacationReview)
            .WithOne(r => r.VacationRequest)
            .HasForeignKey<VacationReview>(r => r.VacationRequestRefId);

        modelBuilder.Entity<VacationReview>().HasOne(v => v.Reviewer).WithMany().OnDelete(DeleteBehavior.NoAction);

        SeedInitialData(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.EnableSensitiveDataLogging();
    }

    public static void SeedInitialData(ModelBuilder modelBuilder)
    {
        const string ADMIN_ID = "923316b1-55f2-4839-96c5-679841e02aff";

        modelBuilder.Entity<IdentityRole>().HasData(InitialData.IdentityRoles);

        var hasher = new PasswordHasher<Employee>();

        modelBuilder.Entity<Position>().HasData(InitialData.Positions);

        var adminEmployee = new
        {
            Id = ADMIN_ID,
            UserName = "admin@outlook.com",
            NormalizedUserName = "admin@outlook.com".Normalize(),
            Email = "admin@outlook.com",
            NormalizedEmail = "admin@outlook.com".Normalize(),
            EmailConfirmed = true,
            PasswordHash = hasher.HashPassword(default!, "password"),
            SecurityStamp = string.Empty,
            AccessFailedCount = 0,
            LockoutEnabled = true,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            Address = "Address",
            DaysOffNumber = 20,
            EmploymentStartDate = DateTime.Now.Date,
            EmploymentEndDate = DateTime.Now.Date.AddDays(365),
            FirstName = "Administrator",
            LastName = "Outlook",
            IdNumber = "12345",
            InsertDate = DateTime.Now,
            PositionId = InitialData.AdminPosition.Id,
            VacationRequests = new List<VacationRequest>(),
            VacationReviews = new List<VacationReview>()
        };

        var defaultEmployee = new
        {
            Id = new StringBuilder(8 + 1 + 4 + 1 + 4 + 1 + 12)
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
            EmailConfirmed = true,
            PasswordHash = hasher.HashPassword(default!, "password"),
            SecurityStamp = string.Empty,
            AccessFailedCount = 0,
            LockoutEnabled = true,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            Address = "Address",
            DaysOffNumber = 20,
            EmploymentStartDate = DateTime.Now.Date,
            EmploymentEndDate = DateTime.Now.Date.AddDays(365),
            FirstName = "Andrija",
            LastName = "Tošić",
            IdNumber = "10000",
            InsertDate = DateTime.Now,
            PositionId = InitialData.Positions[3].Id,
            VacationRequests = new List<VacationRequest>(),
            VacationReviews = new List<VacationReview>()
        };

        modelBuilder.Entity<Employee>().HasData(adminEmployee, defaultEmployee);

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>[] {new IdentityUserRole<string>
            {
                RoleId = InitialData.AdminRole.Id,
                UserId = ADMIN_ID
            },
            new IdentityUserRole<string>
            {
                RoleId = InitialData.DefaultRole.Id,
                UserId = defaultEmployee.Id
            }
        });
    }
}
