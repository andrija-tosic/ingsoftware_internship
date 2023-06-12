using Bogus;
using VacaYAY.API.Models;

namespace VacaYAY.API.Fakes;
public static class EmployeeFaker
{
    public static IList<Employee> GenerateFakes(int count, IList<Position> positions)
    {
        Faker<Employee> faker = new Faker<Employee>()
            .RuleFor(e => e.FirstName, f => f.Person.FirstName)
            .RuleFor(e => e.LastName, f => f.Person.LastName)
            .RuleFor(e => e.Address, f => f.Address.FullAddress())
            .RuleFor(e => e.Email, f => f.Internet.Email(f.Person.FirstName, f.Person.LastName))
            //.RuleFor(e => e.PasswordHash, f => f.Hashids.Encode())
            .RuleFor(e => e.IdNumber, f => GenerateRandomIdNumber())
            .RuleFor(e => e.DaysOffNumber, f => f.Random.Int(0, int.MaxValue))
            .RuleFor(e => e.Position, f => positions[f.Random.Int(0, positions.Count - 1)])
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
}
