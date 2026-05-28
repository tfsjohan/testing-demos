# TypeScript-demo: Jest

Samma koncept som .NET-demot, fast i TypeScript med Jest.

## Köra

```bash
npm install
npm test
```

## Vad varje fil visar

| Fil | Demonstrerar |
|---|---|
| `src/vat.ts` / `vat.test.ts` | Ren funktion, `expect().toBe()`, `test.each` (= `[Theory]`), `toThrow` |
| `src/customer-service.ts` / `customer-service.test.ts` | `jest.Mocked<T>`, `jest.fn`, `mockResolvedValue`, `toHaveBeenCalledWith`, `expect.objectContaining`, `beforeEach` |
