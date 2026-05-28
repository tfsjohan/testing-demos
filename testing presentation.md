# Presentation: Mjukvarutestning för .NET- och TypeScript-utvecklare
**Format:** ~30 minuter, 17 slides
**Målgrupp:** Erfarna .NET/TypeScript-utvecklare som bygger Office.js-tillägg och mindre backends, men är nybörjare på testning.
**Stack i exemplen:** xUnit, NSubstitute, Bogus (.NET) · Jest, ts-jest, office-addin-mock (TypeScript)

---

## Slide 1 — Titel: "Testning från noll — för .NET- och TypeScript-utvecklare"

**Punkter:**
- Vad du får ut av nästa 30 minuter
- Enhetstester, integrationstester, lite mutation
- Verkliga exempel: Office.js, små .NET-backends, TypeScript

**Talarmanus:**
> Hej och välkomna. Idag ska vi prata testning. Ni är duktiga utvecklare, ni har skrivit .NET och TypeScript i flera år, men många av er har aldrig skrivit ett enda enhetstest. Det är inget att skämmas över — det är vanligare än man tror. Målet idag är inte att jag ska övertyga er om att testning är moraliskt rätt, utan att ni ska gå härifrån och kunna skriva ert första rimliga test imorgon bitti. Vi kommer hålla oss till stacken vi faktiskt jobbar med: xUnit i .NET, Jest i TypeScript, NSubstitute för mockning, Bogus för testdata, och office-addin-mock när vi rör Office.js. Avbryt gärna med frågor — det är bättre att vi reder ut något halvvägs än att ni gissar er fram efteråt.

---

## Slide 2 — Varför ens bry sig om tester?

**Punkter:**
- Snabb återkoppling — sekunder istället för "F5 och klicka i Excel"
- Trygghet vid refaktorering
- Levande dokumentation av hur koden *ska* bete sig
- Mindre tid i debuggern, mer tid på faktisk feature-utveckling
- Speciellt för Office-tillägg: F5-loopen är *långsam*

**Talarmanus:**
> Den klassiska invändningen är "vi hinner inte testa". Men tänk på er egen vardag. Hur testar ni idag att en ny funktion i ett Excel-tillägg fungerar? Ni bygger, sideloadar tillägget, väntar på att Excel startar, klickar er fram till rätt cell, ser att det blev fel, lägger en console.log, börjar om. Det är minuter per iteration. Ett enhetstest kör på millisekunder. Den andra stora vinsten kommer sex månader senare när någon ska ändra logiken i en faktureringsregel och inte vågar för att det är okänt vad som händer. Tester är den enda trovärdiga dokumentationen vi har på *hur* koden faktiskt beter sig, för kommentarer ljuger och Confluence är inaktuellt.

---

## Slide 3 — Testpyramiden

**Punkter:**
- Bas: många unit tests (snabba, billiga, isolerade)
- Mitt: färre integrationstester (verifierar att delarna hänger ihop)
- Topp: få E2E-tester (dyra, långsamma, sköra)
- Tumregel: ungefär 70/20/10
- Inverterad pyramid = långsam CI, instabila byggen

**Talarmanus:**
> Begreppet "testpyramid" kommer från Mike Cohns bok *Succeeding with Agile: Software Development Using Scrum* från 2009, och har sedan dess populariserats av bland andra Martin Fowler. Idén håller än. Många tester längst ner där de är billiga, få tester längst upp där de är dyra. En vanlig nybörjarfälla är att hoppa direkt till E2E — "vi kör Playwright mot vår task pane". Det fungerar tills ni har 50 sådana tester och varje CI-bygge tar 45 minuter, hälften av testen är flaky, och ingen litar på röda byggen längre. Lägg er energi i botten av pyramiden först. För våra typer av system — Office-tillägg och små backends — fungerar 70/20/10 som riktmärke ganska bra.

---

## Slide 4 — Översiktstabell: Vilken testtyp gör vad?

**Punkter (tabell):**

| Testtyp | Vad det testar | Hastighet | När använda | Verktyg |
|---|---|---|---|---|
| **Unit test** | En enskild funktion/klass isolerat, dependencies mockas | Millisekunder | Affärslogik, beräkningar, validering, edge-cases | xUnit, Jest |
| **Integration test** | Flera komponenter ihop, ofta inkl. DB/HTTP/filsystem | Sekunder | API-endpoints, repositories, kontrakt mot DB | xUnit + WebApplicationFactory, Testcontainers, Supertest |
| **E2E test** | Hela systemet från användarens perspektiv | Sekunder–minuter | Kritiska användarflöden, "smoke" innan release | Playwright, Cypress; för Office: manuell sideload + Playwright |
| **Mutation test** | Hur bra dina *tester* är (introducerar buggar och ser om testerna fångar dem) | Minuter–timmar | Som komplement, inte ersättare; för kritisk logik | Stryker.NET, StrykerJS |

**Talarmanus:**
> Det här är slidet jag vill att ni fotograferar med mobilen. Fyra kolumner — vad det testar, hur snabbt, när, och med vilka verktyg. Lägg märke till att mutation testing är på en helt annan abstraktionsnivå — det testar inte din kod, det testar dina *tester*. Stryker.NET tar din kod, ändrar ett `>` till ett `>=`, ett `+` till ett `-`, ett `true` till ett `false`, och kör om alla dina tester. Om testerna fortfarande går grönt så har du en "överlevande mutant" — alltså en bugg du själv just införde, som inga av dina tester upptäckte. Det är ett brutalt ärligt mått på testkvalitet. Vi går inte djupare in på mutation idag, men kom ihåg att det finns när någon säger "vi har 95% kodtäckning" — täckning säger ingenting om kvalitet, mutation gör det.

---

## Slide 5 — Enhetstest: AAA-mönstret och namngivning

**Punkter:**
- **Arrange** — sätt upp data och beroenden
- **Act** — kör den ena sak du testar
- **Assert** — verifiera *en* sak
- Namngivning: `Method_Scenario_ExpectedBehavior`
- Ett test = en anledning att misslyckas

**Talarmanus:**
> AAA — Arrange, Act, Assert. Tre tydliga sektioner. Sätt upp, kör, verifiera. Om ert test börjar få fyra block är det inte ett test längre, det är en integrationstest i förklädnad. Namngivningen är viktigare än ni tror. Tänk att ni läser en test-rapport i CI klockan halv åtta på morgonen med kaffekoppen i handen. `Test_1` säger ingenting. `CalculateVat_NegativeAmount_ThrowsArgumentException` säger allt. Mönstret `MethodName_StateUnderTest_ExpectedBehavior` lanserades av Roy Osherove i *The Art of Unit Testing* (Manning) — det är inte det enda gångbara, vissa föredrar BDD-stil med "Should…When…", men det är en mycket bra default som mappar rakt mot AAA.

---

## Slide 6 — Första enhetstestet i C# med xUnit

**Punkter:**
- `[Fact]` för parameterlöst test
- `Assert.Equal`, `Assert.True` osv.
- xUnit kör test i parallell som default
- Klassen ny per test → ingen delad state

**Kodexempel:**

```csharp
// Production code: Services/VatCalculator.cs
public class VatCalculator
{
    public decimal CalculateVat(decimal amount, decimal rate)
    {
        if (amount < 0) throw new ArgumentException("Amount cannot be negative", nameof(amount));
        return Math.Round(amount * rate, 2, MidpointRounding.AwayFromZero);
    }
}

// Test project: Services/VatCalculatorTests.cs
using Xunit;

public class VatCalculatorTests
{
    [Fact]
    public void CalculateVat_StandardRate_ReturnsCorrectAmount()
    {
        // Arrange
        var sut = new VatCalculator();

        // Act
        var result = sut.CalculateVat(100m, 0.25m);

        // Assert
        Assert.Equal(25m, result);
    }

    [Fact]
    public void CalculateVat_NegativeAmount_ThrowsArgumentException()
    {
        var sut = new VatCalculator();

        Assert.Throws<ArgumentException>(() => sut.CalculateVat(-1m, 0.25m));
    }
}
```

**Talarmanus:**
> Det här är det enklaste tänkbara xUnit-testet. `[Fact]` betyder "det här är ett test utan parametrar". xUnit skapar en ny instans av testklassen *för varje test* — det är medvetet, för att inget state ska läcka mellan tester. Variabelnamnet `sut` står för "system under test" och är en bra konvention som visar exakt vad du testar i raden under. För det negativa fallet använder vi `Assert.Throws` som verifierar både att rätt exception-typ kastas och att raden inte bara råkade fungera av andra skäl. Lägg också märke till att vi testar både ett glatt fall och ett felfall — det är poängen med tester, glada fall hittar man ändå manuellt.

---

## Slide 7 — Parametriserade tester med `[Theory]`

**Punkter:**
- `[Theory]` + `[InlineData]` = samma test, många datauppsättningar
- Sparar copy-paste, läsbar test-rapport
- Andra alternativ: `[MemberData]`, `[ClassData]`, `TheoryData<T>`

**Kodexempel:**

```csharp
public class VatCalculatorTheoryTests
{
    [Theory]
    [InlineData(100, 0.25, 25)]
    [InlineData(0, 0.25, 0)]
    [InlineData(99.99, 0.12, 12.00)]
    [InlineData(1, 0.06, 0.06)]
    public void CalculateVat_VariousInputs_ReturnsExpected(decimal amount, decimal rate, decimal expected)
    {
        var sut = new VatCalculator();

        var result = sut.CalculateVat(amount, rate);

        Assert.Equal(expected, result);
    }
}
```

**Talarmanus:**
> `[Theory]` är xUnits sätt att säga "samma testlogik, många indata". Varje `[InlineData]` blir ett separat test i test-rapporten, så om ett edge-case går sönder ser ni exakt vilket. Det här är guld för validering, parsning, datumlogik — överallt där ni annars hade kopierat samma test fem gånger. Begränsningen är att argumenten måste vara kompileringskonstanter. Behöver ni mer komplexa objekt finns `[MemberData]` eller den nyare typsäkra `TheoryData<T1, T2, T3>`.

---

## Slide 8 — Vad är en mock, och varför?

**Punkter:**
- Enheter har dependencies — databas, HTTP, klocka, filsystem
- Vi vill testa *vår* logik, inte SQL Server
- Mock = stand-in som vi kontrollerar
- "Test double" är paraply­begreppet (stubs, fakes, mocks, spies)

**Talarmanus:**
> Den klassiska nybörjarfrågan är "men min klass anropar en databas, hur kan jag testa den?". Svaret är att du inte ska anropa databasen i ett enhetstest. Du ska anropa något som *ser ut som* databasen men som du har full kontroll över. Det är en mock. I .NET-världen är de tre stora libraryna Moq, NSubstitute och FakeItEasy. Vi går på NSubstitute idag — den har den renaste syntaxen och flest "naturliga ord" snarare än lambdor. Viktig sak att veta: mock-bibliotek i .NET fungerar bara mot `interface` eller `virtual` medlemmar. Det är inte en bugg, det är hur .NET fungerar. Det betyder också att ni bör programmera mot interfaces — det är bra design oavsett.

---

## Slide 9 — Mocking i C# med NSubstitute

**Punkter:**
- `Substitute.For<IInterface>()` skapar en mock
- `.Returns(...)` sätter returvärde
- `.Received()` verifierar att metod anropades
- `Arg.Any<T>()`, `Arg.Is<T>(x => ...)` för argument matching

**Kodexempel:**

```csharp
// The system under test
public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task SaveAsync(Customer customer);
}

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string body);
}

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
        if (customer is null) return false;

        customer.IsActive = false;
        await _repo.SaveAsync(customer);
        await _email.SendAsync(customer.Email, "Account deactivated",
            $"Hi {customer.Name}, your account has been deactivated.");
        return true;
    }
}

// The test
using NSubstitute;
using Xunit;

public class CustomerServiceTests
{
    [Fact]
    public async Task DeactivateAsync_ExistingCustomer_SavesAndSendsEmail()
    {
        // Arrange
        var repo = Substitute.For<ICustomerRepository>();
        var email = Substitute.For<IEmailSender>();
        var id = Guid.NewGuid();
        var customer = new Customer { Id = id, Name = "Anna", Email = "anna@example.com", IsActive = true };
        repo.GetByIdAsync(id).Returns(customer);

        var sut = new CustomerService(repo, email);

        // Act
        var result = await sut.DeactivateAsync(id);

        // Assert
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
        repo.GetByIdAsync(Arg.Any<Guid>()).Returns((Customer?)null);

        var sut = new CustomerService(repo, email);

        var result = await sut.DeactivateAsync(Guid.NewGuid());

        Assert.False(result);
        await email.DidNotReceive().SendAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }
}
```

**Talarmanus:**
> Det här är en realistisk situation: en service som hämtar något, ändrar det, sparar det, och skickar ett mail. Tre saker att lägga märke till. Ett: vi mockar `ICustomerRepository` och `IEmailSender` — vi vill inte ha en riktig databas eller en riktig SMTP-server inblandad. Två: `repo.GetByIdAsync(id).Returns(customer)` — det är så NSubstitute uttrycker "när någon frågar efter just det här id:t, returnera det här objektet". Naturlig syntax. Tre: vi verifierar inte bara *resultatet* utan också *beteendet* — att SaveAsync anropades exakt en gång med en customer som har `IsActive = false`, och att mail skickades. I felfallet använder vi `DidNotReceive()` för att verifiera att vi *inte* skickar mail till okända kunder. Det är ett typiskt buggmönster: man returnerar null men gör för mycket innan dess.

---

## Slide 10 — Testdata med Bogus

**Punkter:**
- Realistisk fejkdata utan att skriva `"Lorem ipsum"` för hand
- `Faker<T>` definierar regler per egenskap
- Lokalisering (svensk data!), determinism via seed
- Sparar tid när testobjekt har många properties

**Kodexempel:**

```csharp
using Bogus;

public class CustomerFaker : Faker<Customer>
{
    public CustomerFaker()
    {
        RuleFor(c => c.Id, f => f.Random.Guid());
        RuleFor(c => c.Name, f => f.Name.FullName());
        RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.Name));
        RuleFor(c => c.IsActive, f => f.Random.Bool(0.9f));
        RuleFor(c => c.RegisteredAt, f => f.Date.Past(2));
    }
}

public class OrderFaker : Faker<Order>
{
    public OrderFaker()
    {
        // Swedish locale for realistic addresses/names
        Locale = "sv";
        RuleFor(o => o.OrderId, f => f.IndexFaker);
        RuleFor(o => o.Total, f => f.Finance.Amount(50, 5000));
        RuleFor(o => o.ShippingCity, f => f.Address.City());
    }
}

public class CustomerServiceBogusTests
{
    [Fact]
    public async Task DeactivateAsync_GeneratedCustomer_StillDeactivates()
    {
        // Arrange
        var customer = new CustomerFaker().Generate();
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

// Use Randomizer.Seed = new Random(1234) once at startup for deterministic data.
```

**Talarmanus:**
> Bogus löser ett tråkigt problem: när ni har ett objekt med femton properties och bara två är intressanta för det specifika testet. Istället för att handskriva `new Customer { Id = Guid.NewGuid(), Name = "Test", Email = "test@test.se", ... }` definierar ni en `Faker<T>` en gång och får realistisk data gratis. Det blir både mindre kod *och* tydligare vad som faktiskt spelar roll för testet — det är de properties ni *inte* använder default-värden för. Två tips: använd lokal `"sv"` för svenska namn och adresser, och sätt en seed om ni behöver deterministiska tester. Enligt Bogus officiella GitHub-readme är biblioteket "fundamentally a C# port of faker.js and inspired by FluentValidation's syntax sugar", så om någon av er kör Node-projekt finns motsvarigheten där också.

---

## Slide 11 — Enhetstest i TypeScript med Jest

**Punkter:**
- `describe` / `test` (eller `it`) struktur
- Matchers: `toBe`, `toEqual`, `toThrow`, `toHaveBeenCalledWith`
- TS-Jest eller swc/babel för TypeScript-stöd
- Liknar xUnit i koncept, syntax skiljer

**Kodexempel:**

```typescript
// src/vat.ts
export function calculateVat(amount: number, rate: number): number {
  if (amount < 0) {
    throw new Error("Amount cannot be negative");
  }
  return Math.round(amount * rate * 100) / 100;
}

// src/vat.test.ts
import { calculateVat } from "./vat";

describe("calculateVat", () => {
  test("returns 25 for 100 at 25%", () => {
    expect(calculateVat(100, 0.25)).toBe(25);
  });

  test.each([
    [100, 0.25, 25],
    [0, 0.25, 0],
    [99.99, 0.12, 12.00],
    [1, 0.06, 0.06],
  ])("calculateVat(%f, %f) returns %f", (amount, rate, expected) => {
    expect(calculateVat(amount, rate)).toBe(expected);
  });

  test("throws on negative amount", () => {
    expect(() => calculateVat(-1, 0.25)).toThrow("Amount cannot be negative");
  });
});
```

**Talarmanus:**
> Samma övning igen, men i TypeScript med Jest. `describe` grupperar relaterade tester, `test` (eller `it`, samma sak) är själva testet. `expect(x).toBe(y)` är Jests variant av Assert.Equal. Lägg märke till `test.each` — det är Jests motsvarighet till `[Theory]` med `[InlineData]`. Det är samma idé: en testfunktion, många datauppsättningar, en rad per fall i rapporten. För TypeScript-stöd kör de flesta antingen `ts-jest` eller babel/swc. Är ni i ett Office-tilläggsprojekt skapat med Yeoman-generatorn så är detta redan konfigurerat åt er.

---

## Slide 12 — Mocking i Jest

**Punkter:**
- `jest.fn()` — en mock-funktion
- `jest.mock("module")` — auto-mockar hel modul
- `mockResolvedValue` / `mockReturnValue` för returvärden
- Typad mockning i TS: `jest.Mocked<T>` / `jest.mocked()`

**Kodexempel:**

```typescript
// src/customer-service.ts
export interface CustomerRepository {
  getById(id: string): Promise<Customer | null>;
  save(customer: Customer): Promise<void>;
}

export interface EmailSender {
  send(to: string, subject: string, body: string): Promise<void>;
}

export interface Customer {
  id: string;
  name: string;
  email: string;
  isActive: boolean;
}

export class CustomerService {
  constructor(
    private readonly repo: CustomerRepository,
    private readonly email: EmailSender
  ) {}

  async deactivate(id: string): Promise<boolean> {
    const customer = await this.repo.getById(id);
    if (!customer) return false;

    customer.isActive = false;
    await this.repo.save(customer);
    await this.email.send(customer.email, "Account deactivated",
      `Hi ${customer.name}, your account has been deactivated.`);
    return true;
  }
}

// src/customer-service.test.ts
import { CustomerService, CustomerRepository, EmailSender, Customer } from "./customer-service";

describe("CustomerService.deactivate", () => {
  let repo: jest.Mocked<CustomerRepository>;
  let email: jest.Mocked<EmailSender>;
  let sut: CustomerService;

  beforeEach(() => {
    repo = {
      getById: jest.fn(),
      save: jest.fn(),
    };
    email = {
      send: jest.fn(),
    };
    sut = new CustomerService(repo, email);
  });

  test("deactivates and notifies existing customer", async () => {
    // Arrange
    const customer: Customer = {
      id: "c1", name: "Anna", email: "anna@example.com", isActive: true
    };
    repo.getById.mockResolvedValue(customer);

    // Act
    const result = await sut.deactivate("c1");

    // Assert
    expect(result).toBe(true);
    expect(customer.isActive).toBe(false);
    expect(repo.save).toHaveBeenCalledWith(
      expect.objectContaining({ id: "c1", isActive: false })
    );
    expect(email.send).toHaveBeenCalledWith(
      "anna@example.com",
      "Account deactivated",
      expect.stringContaining("Anna")
    );
  });

  test("returns false and sends no email for unknown customer", async () => {
    repo.getById.mockResolvedValue(null);

    const result = await sut.deactivate("missing");

    expect(result).toBe(false);
    expect(email.send).not.toHaveBeenCalled();
  });
});
```

**Talarmanus:**
> Samma scenario som NSubstitute-exemplet, fast i Jest. Tre saker att notera. Ett: `jest.Mocked<T>` är ett TypeScript-trick som ger oss både typsäkerhet *och* tillgång till mock-API:et `mockResolvedValue`, `toHaveBeenCalledWith` osv. utan att TypeScript klagar. Två: vi sätter upp mockarna i `beforeEach` så att varje test får en ren slate — väldigt likt hur xUnit ger oss en ny klassinstans per test. Tre: `expect.objectContaining` och `expect.stringContaining` är "partial matchers" — vi bryr oss inte om alla fält, bara de relevanta. Det gör testen mindre spröda mot framtida ändringar.

---

## Slide 13 — Integrationstest i .NET med WebApplicationFactory

**Punkter:**
- Startar hela API:et i minnet (TestServer)
- Riktig DI-container, riktig routing, riktig serialisering
- Mocka *externa* dependencies (DB, betaltjänst) via `ConfigureTestServices`
- NuGet: `Microsoft.AspNetCore.Mvc.Testing`
- Lägg `public partial class Program { }` i Program.cs

**Kodexempel:**

```csharp
// In the API project's Program.cs (bottom of file):
public partial class Program { }

// Tests/Integration/CustomersApiTests.cs
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

public class CustomersApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CustomersApiTests(WebApplicationFactory<Program> factory)
    {
        // Replace the real email sender with a mock for all tests in this class
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                // Remove the real registration
                var descriptor = services.Single(d => d.ServiceType == typeof(IEmailSender));
                services.Remove(descriptor);

                // Add a substitute
                var emailMock = Substitute.For<IEmailSender>();
                services.AddSingleton(emailMock);
            });
        });
    }

    [Fact]
    public async Task GetCustomer_ExistingId_Returns200WithCustomer()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/customers/c1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var customer = await response.Content.ReadFromJsonAsync<CustomerDto>();
        Assert.NotNull(customer);
        Assert.Equal("c1", customer!.Id);
    }

    [Fact]
    public async Task DeactivateCustomer_UnknownId_Returns404()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync("/api/customers/does-not-exist/deactivate", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
```

**Talarmanus:**
> Det här är där det börjar bli intressant. WebApplicationFactory startar hela ASP.NET Core-applikationen i minnet — riktig DI-container, riktig routing, riktig middleware-pipeline, riktig modelbinding. Det är *inte* en E2E-test för det är ingen browser inblandad och inget nätverk över sladd, men det är ett mycket bättre integrationstest än att försöka enhetstesta en controller direkt. Två viktiga saker. Ett: ni måste lägga raden `public partial class Program { }` längst ner i ert Program.cs — det är en konsekvens av .NET 6's minimal hosting model, för att test-projektet ska kunna referera Program-typen. Två: `ConfigureTestServices` är magin — där byter ni ut precis de dependencies ni vill kontrollera, ofta externa system. För databasen finns två populära strategier: in-memory provider för Entity Framework (snabb men inte 100 procent trogen) eller Testcontainers (riktig databas i Docker, mycket trogen, lite långsammare). För nya projekt rekommenderar jag Testcontainers när det går.

---

## Slide 14 — Test av Office.js-tillägg

**Punkter:**
- Problemet: Office.js initierar sig själv i en webview *inuti* Excel/Word — det går inte att ladda i Node
- Lösning: `office-addin-mock` — håller intern state, härmar `load`/`sync`
- Funkar med Jest, Mocha — vilken JS-runner som helst
- Installation: `npm install office-addin-mock --save-dev`
- Kräver Node test environment (inte jsdom)

**Kodexempel:**

```typescript
// src/excel/highlight-selection.ts
export async function highlightSelection(): Promise<string> {
  return Excel.run(async (context) => {
    const range = context.workbook.getSelectedRange();
    range.load("address");
    range.format.fill.color = "yellow";
    await context.sync();
    return range.address;
  });
}

// src/excel/highlight-selection.test.ts
import { OfficeMockObject } from "office-addin-mock";
import { highlightSelection } from "./highlight-selection";

const mockData = {
  context: {
    workbook: {
      range: {
        address: "Sheet1!B2:D4",
        format: { fill: {} },
      },
      getSelectedRange: function () {
        return this.range;
      },
    },
  },
  run: async function (callback: (ctx: unknown) => Promise<void>) {
    await callback(this.context);
  },
};

describe("highlightSelection", () => {
  test("paints the selected range yellow", async () => {
    // Arrange
    const excelMock = new OfficeMockObject(mockData);
    (global as any).Excel = excelMock;

    // Act
    await highlightSelection();

    // Assert
    expect((excelMock as any).context.workbook.range.format.fill.color)
      .toBe("yellow");
  });
});
```

**Talarmanus:**
> Microsoft Learn säger ordagrant: "The Office JavaScript APIs must initialize in a webview control in the context of an Office application (such as Excel, PowerPoint, or Word), so they can't load in the process in which unit tests run on your development computer." Det är problemet. Lösningen är `office-addin-mock` — ett bibliotek från Microsoft som ger er ett OfficeMockObject som faktiskt håller intern state om vad som load:ats och sync:ats, så att felmeddelanden härmar verkligheten. Ni bygger upp ett "seed object" som beskriver den lilla del av Office-objektmodellen testet rör. En viktig fallgrop: om ert projekt sätter `testEnvironment: "jsdom"` i Jest-konfigen — vanligt i React/task pane-projekt — så fungerar inte office-addin-mock direkt eftersom det internt använder Node-API:er. Antingen sätt `testEnvironment: "node"` per fil med en docblock-kommentar `/** @jest-environment node */`, eller separera era Office-API-tester i en egen jest-projektkonfig. Det viktigaste tipset är ändå: bryt ut så mycket affärslogik som möjligt till rena funktioner som inte rör Office-objekten alls — då slipper ni office-addin-mock i merparten av era tester.

---

## Slide 15 — Praktiska best practices

**Punkter:**
- En anledning att misslyckas per test
- Inga sleeps, inga riktiga klockor — injicera `IClock` / `Date.now`-wrapper
- Inga ordningsberoenden mellan tester
- Snabba tester på varje commit, långsammare nattligt
- Mocka *external boundaries*, inte er egen domänlogik
- Tester ska skrivas så att en annan utvecklare förstår intent på 30 sekunder
- FluentAssertions (`Should().Be(...)`) i .NET för läsbarhet, om laget gillar det

**Talarmanus:**
> Några erfarenhetsregler att ta med sig. Mocka aldrig er egen domänlogik — om ni mockar in ett `IDiscountCalculator` och bara säger "returnera 10" så testar ni egentligen ingenting alls. Mocka istället över *gränsen* mot omvärlden: databasen, HTTP, klockan, filsystemet, mail-tjänsten. Tester med Thread.Sleep eller setTimeout är en kodlukt — det betyder att ni testar timing och då kommer testen vara flaky. Injicera en klocka istället. Och — det här är kanske det viktigaste rådet — skriv testet som om personen som ska läsa det är trött, det är fredag eftermiddag, och produktionen brinner. Namn, struktur, en sak åt gången.

---

## Slide 16 — Var börjar man imorgon?

**Punkter:**
- Välj *en* befintlig klass med riktig logik (validering, beräkning, parsning)
- Skriv tre tester: glad väg, fel-input, edge case
- När du nästa gång stöter på en bugg → skriv testet *innan* du fixar
- För Office-tillägg: bryt ut affärslogik till rena funktioner, testa dem; mocka Office bara för pjäserna som faktiskt rör Excel/Word
- Mål för månad 1: kunna refaktorera utan rädsla i minst en modul

**Talarmanus:**
> Sista slidet. Gå inte härifrån och försök testa allt på måndag. Välj en klass. Helst en med riktig logik — en validator, en beräkning, en parser. Skriv tre tester. När ni nästa vecka stöter på en bugg, skriv testet som reproducerar buggen *innan* ni fixar. Då har ni dels en garanti att fixen funkar, dels en garanti att buggen aldrig kommer tillbaka. För Office-tillägg specifikt — det viktigaste tipset är att bryta ut affärslogiken till rena funktioner som inte rör Office.Context alls. De funktionerna kan ni testa hur lätt som helst utan att mocka något. Spara office-addin-mock till just de pjäser som faktiskt anropar Excel- eller Word-API:erna.

---

## Slide 17 — Resurser & frågor

**Punkter:**
- xUnit docs — xunit.net
- NSubstitute docs — nsubstitute.github.io
- Bogus — github.com/bchavez/Bogus
- Jest — jestjs.io
- Microsoft Learn: "Unit testing in Office Add-ins"
- Microsoft Learn: "Integration tests in ASP.NET Core"
- Stryker.NET — för när ni är redo att gå ett steg djupare
- **Frågor?**

**Talarmanus:**
> Det här är de officiella källorna jag använt för att förbereda — alla har bra exempel och uppdaterade rekommendationer. Microsoft Learn-sidan om Office-tilläggstestning är den enda riktigt bra resursen för exakt vår nisch, så bokmärk den. Tack för att ni lyssnat — vilka frågor har ni?