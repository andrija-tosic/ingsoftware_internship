using Bogus;
using VacaYAY.Data.Models;

namespace VacaYAY.Business.Fakes;
public class BogusFaker
{
    public static IList<Employee> GenerateFakeEmployees(int count)
    {   
        Faker<Employee> faker = new Faker<Employee>()
            .RuleFor(e => e.FirstName, f => f.Person.FirstName)
            .RuleFor(e => e.LastName, f => f.Person.LastName)
            .RuleFor(e => e.Address, f => f.Address.FullAddress())
            .RuleFor(e => e.Email, f => f.Internet.Email(f.Person.FirstName, f.Person.LastName))
            //.RuleFor(e => e.PasswordHash, f => f.Hashids.Encode())
            .RuleFor(e => e.IdNumber, f => GenerateRandomIdNumber())
            .RuleFor(e => e.DaysOffNumber, f => f.Random.Int(0, int.MaxValue))
            .RuleFor(e => e.Position, GenerateRandomPosition)
            .RuleFor(e => e.EmploymentStartDate, f => f.Date.Past())
            .RuleFor(e => e.EmploymentEndDate, f => f.Date.Recent().OrNull(f))
            .RuleFor(e => e.InsertDate, f => f.Date.Past())
            .RuleFor(e => e.DeleteDate, f => null)
            .RuleFor(e => e.VacationRequests, f => new List<VacationRequest>());

        IList<Employee> e = faker.Generate(count);
        
        return e;
    }

    private static string GenerateRandomIdNumber()
    {
        var idNumber = new Faker().Random.Int(10000000, 99999999).ToString();
        return idNumber;
    }

    private static Position GenerateRandomPosition(Faker f)
    {
        return new Position
        {
            Id = default!,
            Caption = f.Name.JobTitle(),
            Description = f.Name.JobDescriptor(),
            Employees = new List<Employee>()
        };
    }

    private static IList<VacationRequest> GenerateRandomVacationRequests(Faker f)
    {
        var vacationRequests = new List<VacationRequest>();
        var numRequests = f.Random.Int(0, 10);

        for (int i = 0; i < numRequests; i++)
        {
            var request = new VacationRequest
            {
                Id = default!,
                Employee = GenerateFakeEmployees(1).Single(),
                Comment = f.Lorem.Sentence(),
                StartDate = DateTime.Now.AddDays(f.Random.Int(1, 30)),
                EndDate = DateTime.Now.AddDays(f.Random.Int(30, 60))
            };

            vacationRequests.Add(request);
        }

        return vacationRequests;
    }
}
