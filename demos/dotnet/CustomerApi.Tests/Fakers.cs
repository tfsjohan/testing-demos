using Bogus;
using CustomerApi.Models;

namespace CustomerApi.Tests;

// Bogus: definiera reglerna en gång, få realistisk testdata gratis.
// Poängen blir att testet bara behöver bry sig om de fält som spelar roll —
// resten fylls i automatiskt.
public class CustomerFaker : Faker<Customer>
{
    public CustomerFaker()
    {
        RuleFor(c => c.Id, f => f.Random.Guid());
        RuleFor(c => c.Name, f => f.Name.FullName());
        RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.Name));
        RuleFor(c => c.IsActive, f => f.Random.Bool(0.9f)); // 90% aktiva
        RuleFor(c => c.RegisteredAt, f => f.Date.Past(2));
    }
}
