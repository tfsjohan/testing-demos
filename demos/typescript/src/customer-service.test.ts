import {
  CustomerService,
  CustomerRepository,
  EmailSender,
  Customer,
} from "./customer-service";

describe("CustomerService.deactivate", () => {
  // jest.Mocked<T> ger oss typsäkerhet OCH mock-API (mockResolvedValue m.m.).
  let repo: jest.Mocked<CustomerRepository>;
  let email: jest.Mocked<EmailSender>;
  let sut: CustomerService;

  // beforeEach ger varje test en ren slate — likt en ny klassinstans i xUnit.
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
      id: "c1",
      name: "Anna",
      email: "anna@example.com",
      isActive: true,
    };
    repo.getById.mockResolvedValue(customer);

    // Act
    const result = await sut.deactivate("c1");

    // Assert — resultat OCH beteende.
    expect(result).toBe(true);
    expect(customer.isActive).toBe(false);
    // Partial matchers: vi bryr oss bara om de relevanta fälten.
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
