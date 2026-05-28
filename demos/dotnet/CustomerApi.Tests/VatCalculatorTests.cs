using CustomerApi.Services;

namespace CustomerApi.Tests;

// Det enklaste tänkbara enhetstestet: ingen mockning, inga beroenden.
// Mönstret är AAA — Arrange, Act, Assert.
public class VatCalculatorTests
{
    [Fact]
    public void CalculateVat_StandardRate_ReturnsCorrectAmount()
    {
        // Arrange — sätt upp det vi behöver (sut = "system under test").
        var sut = new VatCalculator();

        // Act — kör den ENA sak vi testar.
        var result = sut.CalculateVat(100m, 0.25m);

        // Assert — verifiera EN sak.
        Assert.Equal(25m, result);
    }

    [Fact]
    public void CalculateVat_NegativeAmount_ThrowsArgumentException()
    {
        var sut = new VatCalculator();

        // Felfallet är minst lika viktigt som det glada fallet.
        Assert.Throws<ArgumentException>(() => sut.CalculateVat(-1m, 0.25m));
    }

    // [Theory] = samma testlogik, många datauppsättningar.
    // Varje [InlineData] blir ett eget test i rapporten.
    [Theory]
    [InlineData(100, 0.25, 25)]
    [InlineData(0, 0.25, 0)]
    [InlineData(99.99, 0.12, 12.00)]
    [InlineData(1, 0.06, 0.06)]
    public void CalculateVat_VariousInputs_ReturnsExpected(decimal amount, decimal rate, decimal expected)
    {
        var sut = new VatCalculator();

        var result = sut.CalculateVat(amount, rate);

        Assert.Equal(expected, result);
    }
}
