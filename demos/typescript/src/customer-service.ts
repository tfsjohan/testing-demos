export interface Customer {
  id: string;
  name: string;
  email: string;
  isActive: boolean;
}

// Beroenden uttrycks som interfaces, precis som i .NET-demot.
export interface CustomerRepository {
  getById(id: string): Promise<Customer | null>;
  save(customer: Customer): Promise<void>;
}

export interface EmailSender {
  send(to: string, subject: string, body: string): Promise<void>;
}

export class CustomerService {
  // Beroenden in via konstruktorn -> lätt att skicka in mockar i test.
  constructor(
    private readonly repo: CustomerRepository,
    private readonly email: EmailSender
  ) {}

  async deactivate(id: string): Promise<boolean> {
    const customer = await this.repo.getById(id);
    if (!customer) return false; // okänd kund -> gör inget mer

    customer.isActive = false;
    await this.repo.save(customer);
    await this.email.send(
      customer.email,
      "Account deactivated",
      `Hi ${customer.name}, your account has been deactivated.`
    );
    return true;
  }
}
