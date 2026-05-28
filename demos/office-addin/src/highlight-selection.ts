// Den här funktionen rör faktiskt Excel-API:et, så den är svårare att testa.
// Här (och bara här) behöver vi office-addin-mock.
export async function highlightSelection(): Promise<string> {
  return Excel.run(async (context) => {
    const range = context.workbook.getSelectedRange();
    // load + sync måste anropas innan vi läser en property — annars
    // kastar både Excel OCH mocken ett "property not loaded"-fel.
    range.load("address");
    range.format.fill.color = "yellow";
    await context.sync();
    return range.address;
  });
}
