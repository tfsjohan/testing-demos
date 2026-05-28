namespace CustomerApi.Services;

// Ren affärslogik utan beroenden: lättast tänkbara sak att enhetstesta.
// Inget databas, ingen HTTP, ingen klocka — bara in -> ut.
public class VatCalculator
{
    public decimal CalculateVat(decimal amount, decimal rate)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        // AwayFromZero = "round half up", som folk förväntar sig på en faktura.
        return Math.Round(amount * rate, 2, MidpointRounding.AwayFromZero);
    }
}
