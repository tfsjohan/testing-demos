using CustomerApi.Models;

namespace CustomerApi.Services;

// Klassisk "orchestrator": hämtar, ändrar, sparar, notifierar.
// Beroenden tas in via konstruktorn (dependency injection) så att
// vi kan skicka in mockar i testerna istället för riktig DB och SMTP.
public class CustomerService
{
    private readonly ICustomerRepository _repo;
    private readonly IEmailSender _email;

    public CustomerService(ICustomerRepository repo, IEmailSender email)
    {
        _repo = repo;
        _email = email;
    }

    public async Task<bool> DeactivateAsync(Guid id)
    {
        var customer = await _repo.GetByIdAsync(id);
        if (customer is null)
            return false; // Okänd kund -> gör inget mer, skicka inget mail.

        customer.IsActive = false;
        await _repo.SaveAsync(customer);
        await _email.SendAsync(
            customer.Email,
            "Account deactivated",
            $"Hi {customer.Name}, your account has been deactivated.");
        return true;
    }
}
