/** @jest-environment node */
// ^ Docblocken tvingar Node-miljö för just den här filen. Behövs om projektets
// jest.config sätter testEnvironment: "jsdom" (vanligt i React/task pane-projekt).

import { OfficeMockObject } from "office-addin-mock";
import { highlightSelection } from "./highlight-selection";

// "Seed object": beskriv bara den lilla del av Office-objektmodellen testet rör.
// highlightSelection anropar Excel.run, så vi mockar Host-objektet Excel.
// Då måste vi lägga till run() själva — OfficeMockObject lägger inte till den.
const mockData = {
  context: {
    workbook: {
      range: {
        address: "Sheet1!B2:D4",
        format: { fill: {} },
      },
      getSelectedRange: function () {
        return this.range;
      },
    },
  },
  run: async function (callback: (ctx: any) => Promise<string>) {
    // Returnera callbackens värde — precis som riktiga Excel.run gör.
    return await callback(this.context);
  },
};

describe("highlightSelection", () => {
  test("paints the selected range yellow and returns its address", async () => {
    // Arrange — gör mocken global, precis som Office.js är i produktion.
    const excelMock = new OfficeMockObject(mockData) as any;
    (global as any).Excel = excelMock;

    // Act
    const address = await highlightSelection();

    // Assert
    expect(address).toBe("Sheet1!B2:D4");
    expect(excelMock.context.workbook.range.format.fill.color).toBe("yellow");
  });
});
