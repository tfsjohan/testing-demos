# .NET-demo: xUnit, NSubstitute, Bogus

Minimal web-API med affärslogik byggd för att vara lätt att testa.

## Köra

```bash
dotnet run --project CustomerApi      # startar API:et
dotnet test                           # kör alla tester
```

Test-endpoints (när API:et kör):
- `GET  /api/customers/11111111-1111-1111-1111-111111111111` → 200
- `POST /api/customers/{id}/deactivate`
- `POST /api/vat`  body: `{ "amount": 100, "rate": 0.25 }`

## Vad varje fil visar

| Fil | Demonstrerar |
|---|---|
| `CustomerApi/Services/VatCalculator.cs` | Ren logik utan beroenden |
| `CustomerApi.Tests/VatCalculatorTests.cs` | `[Fact]`, `[Theory]`, AAA, felfall |
| `CustomerApi/Services/CustomerService.cs` | Beroenden via konstruktorn (DI) |
| `CustomerApi.Tests/CustomerServiceTests.cs` | NSubstitute: `.Returns`, `.Received`, `.DidNotReceive`, `Arg.Is` |
| `CustomerApi.Tests/Fakers.cs` + `CustomerServiceBogusTests.cs` | Bogus `Faker<T>` |
| `CustomerApi.Tests/CustomersApiTests.cs` | Integrationstest: `WebApplicationFactory`, `ConfigureTestServices` |
