using CustomerApi.Services;
using NSubstitute;

namespace CustomerApi.Tests;

// Samma scenario som CustomerServiceTests, men kunden kommer från Bogus.
// Vi ser tydligt att Name/Email/RegisteredAt inte spelar roll för logiken —
// bara att kunden finns och att vi avaktiverar den.
public class CustomerServiceBogusTests
{
    [Fact]
    public async Task DeactivateAsync_GeneratedCustomer_StillDeactivates()
    {
        // Arrange
        var customer = new CustomerFaker().Generate();
        customer.IsActive = true; // tvinga utgångsläget vi vill testa
        var repo = Substitute.For<ICustomerRepository>();
        var email = Substitute.For<IEmailSender>();
        repo.GetByIdAsync(customer.Id).Returns(customer);
        var sut = new CustomerService(repo, email);

        // Act
        var result = await sut.DeactivateAsync(customer.Id);

        // Assert
        Assert.True(result);
        Assert.False(customer.IsActive);
    }
}
