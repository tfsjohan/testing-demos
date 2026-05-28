# Demos — Testning från noll

Tre körbara demoprojekt som hör till presentationen. Alla tester är gröna.

| Mapp | Stack | Kör |
|---|---|---|
| [`dotnet/`](dotnet/) | xUnit, NSubstitute, Bogus, WebApplicationFactory | `dotnet test` |
| [`typescript/`](typescript/) | Jest, ts-jest | `npm install && npm test` |
| [`office-addin/`](office-addin/) | Jest, office-addin-mock | `npm install && npm test` |

## Röd tråd mellan demosen

Samma två exempel går igen i alla språk så att åhörarna känner igen sig:

1. **`calculateVat` / `VatCalculator`** — ren funktion, inga beroenden. Visar
   AAA-mönstret, parametriserade tester (`[Theory]` / `test.each`) och felfall.
2. **`CustomerService.deactivate`** — beroenden (repository + mail) mockas.
   Visar att man verifierar både *resultat* och *beteende*.

Office-demot visar dessutom huvudpoängen för Office.js-tillägg: bryt ut logik
till rena funktioner som testas utan mock, och spara `office-addin-mock` till
just de funktioner som faktiskt anropar Excel/Word.

## Not om körning i sandlåda

Jest kan hänga om testkörarens worker-processer blockeras. Kör då i ett enda
tråd: `npx jest --runInBand`.
