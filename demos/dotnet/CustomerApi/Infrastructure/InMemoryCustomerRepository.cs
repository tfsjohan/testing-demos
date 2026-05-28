using System.Collections.Concurrent;
using CustomerApi.Models;
using CustomerApi.Services;

namespace CustomerApi.Infrastructure;

// En enkel "riktig" implementation så att API:et faktiskt kör.
// I testerna byter vi ut den (eller mockar interfacet) — poängen är att
// koden vi testar aldrig vet vilken implementation den pratar med.
public class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly ConcurrentDictionary<Guid, Customer> _customers = new();

    public InMemoryCustomerRepository()
    {
        // En förseedad kund med ett känt id, så att GET-endpointen har data att visa.
        var seed = new Customer
        {
            Id = WellKnownIds.SampleCustomer,
            Name = "Anna Svensson",
            Email = "anna@example.com",
            IsActive = true,
            RegisteredAt = DateTime.UtcNow.AddMonths(-3)
        };
        _customers[seed.Id] = seed;
    }

    public Task<Customer?> GetByIdAsync(Guid id)
    {
        _customers.TryGetValue(id, out var customer);
        return Task.FromResult(customer);
    }

    public Task SaveAsync(Customer customer)
    {
        _customers[customer.Id] = customer;
        return Task.CompletedTask;
    }
}

public static class WellKnownIds
{
    // Ett fast id gör det enkelt att anropa /api/customers/{id} i demot.
    public static readonly Guid SampleCustomer = Guid.Parse("11111111-1111-1111-1111-111111111111");
}
