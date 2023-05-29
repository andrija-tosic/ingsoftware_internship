using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VacaYAY.Data.Models;

namespace VacaYAY.Data;

public class VacayayDbContext : IdentityDbContext<Employee>
{
    public required DbSet<Employee> Employees { get; set; }
    public required DbSet<Position> Positions { get; set; }
    public required DbSet<VacationRequest> VacationRequests { get; set; }
    public required DbSet<VacationRequestReview> VacationRequestsReviews { get; set; }
    public required DbSet<LeaveType> LeaveTypes { get; set; }
    public VacayayDbContext(DbContextOptions<VacayayDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.IdNumber)
            .IsUnique(true);

        modelBuilder.Entity<LeaveType>()
            .HasIndex(e => e.Name)
            .IsUnique(true);

        modelBuilder.Entity<Employee>().ToTable(nameof(Employee) + "s");
    }
}
