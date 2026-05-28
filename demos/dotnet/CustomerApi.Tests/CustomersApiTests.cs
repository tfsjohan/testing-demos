using System.Net;
using System.Net.Http.Json;
using CustomerApi.Infrastructure;
using CustomerApi.Models;
using CustomerApi.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CustomerApi.Tests;

// Integrationstest: startar HELA API:et i minnet — riktig DI, routing,
// modelbinding och JSON-serialisering. Ingen browser och inget nätverk,
// så det är fortfarande snabbt och stabilt.
public class CustomersApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CustomersApiTests(WebApplicationFactory<Program> factory)
    {
        // Byt ut den riktiga mail-sendern mot en mock för alla tester i klassen.
        // Så slipper vi en SMTP-server, och kan verifiera anrop om vi vill.
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.Single(d => d.ServiceType == typeof(IEmailSender));
                services.Remove(descriptor);
                services.AddSingleton(Substitute.For<IEmailSender>());
            });
        });
    }

    [Fact]
    public async Task GetCustomer_ExistingId_Returns200WithCustomer()
    {
        // Arrange
        var client = _factory.CreateClient();
        var id = WellKnownIds.SampleCustomer;

        // Act
        var response = await client.GetAsync($"/api/customers/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var customer = await response.Content.ReadFromJsonAsync<CustomerDto>();
        Assert.NotNull(customer);
        Assert.Equal(id.ToString(), customer!.Id);
    }

    [Fact]
    public async Task GetCustomer_UnknownId_Returns404()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync($"/api/customers/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeactivateCustomer_UnknownId_Returns404()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync($"/api/customers/{Guid.NewGuid()}/deactivate", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CalculateVat_ViaEndpoint_ReturnsComputedValue()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/vat", new { amount = 100m, rate = 0.25m });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<VatResponse>();
        Assert.Equal(25m, body!.Vat);
    }

    private record VatResponse(decimal Vat);
}
