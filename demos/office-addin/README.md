# Office.js-demo: testa en task pane-funktion

Visar hur man testar kod som anropar Office.js (här: Excel) utan att starta Excel.

## Köra

```bash
npm install
npm test
```

## Huvudpoängen

Bryt ut affärslogik till **rena funktioner** (`highlight-rules.ts`) — de testas
utan någon mock alls. Spara `office-addin-mock` till just de funktioner som
faktiskt anropar Excel/Word-API:erna (`highlight-selection.ts`).

| Fil | Demonstrerar |
|---|---|
| `src/highlight-rules.ts` / `.test.ts` | Ren logik — testas direkt, ingen mock |
| `src/highlight-selection.ts` | Funktion som anropar `Excel.run` |
| `src/highlight-selection.test.ts` | `OfficeMockObject`, seed object, `load`/`sync`, `/** @jest-environment node */` |

## Fallgrop

`office-addin-mock` använder Node-API:er internt och fungerar inte under
`testEnvironment: "jsdom"`. Lös det per fil med docblocken
`/** @jest-environment node */` (se testfilen), eller sätt `node` globalt i
`jest.config.js`.

Källa: [Microsoft Learn — Unit testing in Office Add-ins](https://learn.microsoft.com/office/dev/add-ins/testing/unit-testing).
