using CustomerApi.Models;
using CustomerApi.Services;
using NSubstitute;

namespace CustomerApi.Tests;

// Här har koden beroenden (repository + mail). Vi mockar dem med NSubstitute
// så att vi testar VÅR logik — inte en riktig databas eller SMTP-server.
public class CustomerServiceTests
{
    [Fact]
    public async Task DeactivateAsync_ExistingCustomer_SavesAndSendsEmail()
    {
        // Arrange
        var repo = Substitute.For<ICustomerRepository>();
        var email = Substitute.For<IEmailSender>();
        var id = Guid.NewGuid();
        var customer = new Customer
        {
            Id = id, Name = "Anna", Email = "anna@example.com", IsActive = true
        };
        // "När någon frågar efter just det här id:t, returnera den här kunden."
        repo.GetByIdAsync(id).Returns(customer);

        var sut = new CustomerService(repo, email);

        // Act
        var result = await sut.DeactivateAsync(id);

        // Assert — vi verifierar både resultatet OCH beteendet.
        Assert.True(result);
        Assert.False(customer.IsActive);
        await repo.Received(1).SaveAsync(Arg.Is<Customer>(c => !c.IsActive));
        await email.Received(1).SendAsync("anna@example.com", "Account deactivated", Arg.Any<string>());
    }

    [Fact]
    public async Task DeactivateAsync_UnknownCustomer_ReturnsFalseAndSendsNoEmail()
    {
        var repo = Substitute.For<ICustomerRepository>();
        var email = Substitute.For<IEmailSender>();
        // Ingen kund hittas.
        repo.GetByIdAsync(Arg.Any<Guid>()).Returns((Customer?)null);

        var sut = new CustomerService(repo, email);

        var result = await sut.DeactivateAsync(Guid.NewGuid());

        // DidNotReceive() = "vi skickade INTE mail till en okänd kund".
        Assert.False(result);
        await email.DidNotReceive().SendAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }
}
