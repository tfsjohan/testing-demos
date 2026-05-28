import { colorForAmount } from "./highlight-rules";

// Ingen office-addin-mock behövs — det här är ren logik.
// Snabbt, läsbart, och täcker alla grenar.
describe("colorForAmount", () => {
  test.each([
    [-100, "red"],
    [-0.01, "red"],
    [0, "yellow"],
    [0.01, "green"],
    [5000, "green"],
  ])("colorForAmount(%f) -> %s", (amount, expected) => {
    expect(colorForAmount(amount)).toBe(expected);
  });
});
