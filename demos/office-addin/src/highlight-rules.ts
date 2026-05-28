// VIKTIGASTE TIPSET för Office-tillägg: bryt ut affärslogik till rena
// funktioner som inte rör Office-objekten alls. Då slipper du mocka något
// i merparten av dina tester — det här går att testa direkt.

export type FillColor = "red" | "yellow" | "green";

// Vilken färg ska en cell få utifrån sitt belopp?
export function colorForAmount(amount: number): FillColor {
  if (amount < 0) return "red"; // negativt = varning
  if (amount === 0) return "yellow"; // noll = neutralt
  return "green"; // positivt = ok
}
