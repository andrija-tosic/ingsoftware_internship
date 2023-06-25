using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VacaYAY.Data.Models;

namespace VacaYAY.Data;

public class VacayayDbContext : IdentityDbContext<Employee>
{
    public DbSet<Employee> Employees { get; set; } = default!;
    public DbSet<Position> Positions { get; set; } = default!;
    public DbSet<VacationRequest> VacationRequests { get; set; } = default!;
    public DbSet<VacationReview> VacationReviews { get; set; } = default!;
    public DbSet<LeaveType> LeaveTypes { get; set; } = default!;
    public DbSet<ContractType> ContractTypes { get; set; } = default!;
    public DbSet<Contract> Contracts { get; set; } = default!;

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
        modelBuilder.Entity<Contract>().HasQueryFilter(c => c.Employee.DeleteDate == null);

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
        optionsBuilder.UseValidationCheckConstraints(opts => opts.UseRegex(false));
    }

    public static void SeedInitialData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(InitialData.IdentityRoles);

        modelBuilder.Entity<Position>().HasData(InitialData.Positions);

        modelBuilder.Entity<Employee>().HasData(InitialData.Employees);

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = InitialData.AdminIdentityRole.Id,
            UserId = InitialData.AdminEmployee.Id
        });

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(InitialData.Employees
            .Where(e => e.Id != InitialData.AdminEmployee.Id)
            .Select(employee => new IdentityUserRole<string>
            {
                RoleId = InitialData.DefaultIdentityRole.Id,
                UserId = employee.Id
            }));

        modelBuilder.Entity<LeaveType>().HasData(InitialData.LeaveTypes);

        modelBuilder.Entity<ContractType>().HasData(InitialData.ContractTypes);
        modelBuilder.Entity<Contract>().HasData(InitialData.Contracts);
    }
}
