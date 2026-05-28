using CustomerApi.Models;

namespace CustomerApi.Services;

// Vi programmerar mot interfaces. Det är dessa som NSubstitute kan ersätta
// med mockar i testerna — mock-bibliotek i .NET fungerar bara mot
// interface eller virtuella medlemmar.

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task SaveAsync(Customer customer);
}

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string body);
}
