import { calculateVat } from "./vat";

// describe grupperar relaterade tester. test (eller it) är själva testet.
describe("calculateVat", () => {
  test("returns 25 for 100 at 25%", () => {
    expect(calculateVat(100, 0.25)).toBe(25);
  });

  // test.each = Jests motsvarighet till [Theory] + [InlineData] i xUnit.
  test.each([
    [100, 0.25, 25],
    [0, 0.25, 0],
    [99.99, 0.12, 12.0],
    [1, 0.06, 0.06],
  ])("calculateVat(%f, %f) returns %f", (amount, rate, expected) => {
    expect(calculateVat(amount, rate)).toBe(expected);
  });

  test("throws on negative amount", () => {
    expect(() => calculateVat(-1, 0.25)).toThrow("Amount cannot be negative");
  });
});
