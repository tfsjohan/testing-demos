// Ren funktion utan beroenden — motsvarar VatCalculator i .NET-demot.
export function calculateVat(amount: number, rate: number): number {
  if (amount < 0) {
    throw new Error("Amount cannot be negative");
  }
  // Avrunda till två decimaler (ören).
  return Math.round(amount * rate * 100) / 100;
}
