using CustomerApi.Infrastructure;
using CustomerApi.Models;
using CustomerApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrera våra tjänster i DI-containern.
builder.Services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();
builder.Services.AddSingleton<IEmailSender, ConsoleEmailSender>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddSingleton<VatCalculator>();

var app = builder.Build();

// GET /api/customers/{id} -> 200 med kund, eller 404.
app.MapGet("/api/customers/{id}", async (string id, ICustomerRepository repo) =>
{
    if (!Guid.TryParse(id, out var guid))
        return Results.NotFound();

    var customer = await repo.GetByIdAsync(guid);
    return customer is null
        ? Results.NotFound()
        : Results.Ok(new CustomerDto(
            customer.Id.ToString(), customer.Name, customer.Email, customer.IsActive));
});

// POST /api/customers/{id}/deactivate -> 200 om kunden fanns, annars 404.
app.MapPost("/api/customers/{id}/deactivate", async (string id, CustomerService service) =>
{
    if (!Guid.TryParse(id, out var guid))
        return Results.NotFound();

    var ok = await service.DeactivateAsync(guid);
    return ok ? Results.Ok() : Results.NotFound();
});

// POST /api/vat -> beräknar moms. Visar att även "ren" logik kan exponeras.
app.MapPost("/api/vat", (VatRequest req, VatCalculator calculator) =>
{
    try
    {
        var vat = calculator.CalculateVat(req.Amount, req.Rate);
        return Results.Ok(new { vat });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.Run();

public record VatRequest(decimal Amount, decimal Rate);

// Behövs för att testprojektet ska kunna referera Program-typen i
// WebApplicationFactory<Program>. Konsekvens av .NET:s minimal hosting model.
public partial class Program { }
