using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
    }
}
