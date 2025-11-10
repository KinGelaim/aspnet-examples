using Shouldly;

namespace Testing.Shouldly;

public class CalculatorTests
{
    private Calculator Sut()
    {
        return new Calculator();
    }

    [Fact]
    public void Add_ReturnsSum()
    {
        // Arrange
        var firstNumber = 1;
        var secondNumber = 2;
        var expectedResult = 3;

        // Act
        var actualResult = Sut().Add(firstNumber, secondNumber);

        // Assert
        actualResult.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData(4, 2, 2)]
    [InlineData(9, 3, 3)]
    [InlineData(9, -3, -3)]
    public void Division_ReturnsDivisionResult(double numerator, double denominator, double expectedResult)
    {
        // Act
        var actualResult = Sut().Division(numerator, denominator);

        // Assert
        actualResult.ShouldBe(expectedResult);
    }

    [Fact]
    public void Division_ReturnsDivisionResult_WhenResultHasInaccuracy()
    {
        // Arrange
        var numerator = 10;
        var denominator = 3;
        var expectedResult = 3.333;

        // Act
        var actualResult = Sut().Division(numerator, denominator);

        // Assert
        actualResult.ShouldBe(expectedResult, 0.001);
    }

    [Fact]
    public void Division_ReturnsArgumentOutOfRangeException_WhenDenominatorIsZero()
    {
        // Arrange
        var numerator = 1;
        var denominator = 0;

        // Act and assert
        var ex = Should.Throw<ArgumentOutOfRangeException>(() => Sut().Division(numerator, denominator));
        ex.Message.ShouldBe("denominator ('0') must be a non-zero value. (Parameter 'denominator')\r\nActual value was 0.");
    }

    [Theory]
    [MemberData(nameof(GetEnumerableValues))]
    public void Division_ReturnsDivisionResult_WithEnumerableValues(
        double numerator,
        double denominator,
        double expectedResult)
    {
        // Act
        var actualResult = Sut().Division(numerator, denominator);

        // Assert
        actualResult.ShouldBe(expectedResult);
    }

    public static IEnumerable<object[]> GetEnumerableValues =>
    [
        [10, 5, 2],
        [-120, 2, -60]
    ];

    [Theory]
    [MemberData(nameof(GetDivisionTheoryData), parameters: [3, 1, 20])]
    public void Division_ReturnsDivisionResult_WithGeneratedValues(
        double numerator,
        double denominator,
        double expectedResult)
    {
        // Act
        var actualResult = Sut().Division(numerator, denominator);

        // Assert
        actualResult.ShouldBe(expectedResult);
    }

    public static TheoryData<double, double, double> GetDivisionTheoryData(
        int testsCount,
        int minLength,
        int maxLength)
    {
        var data = new TheoryData<double, double, double>();

        var random = new Random();
        for (var i = 0; i < testsCount; i++)
        {
            var value = (double)random.Next(minLength, maxLength);

            data.Add(value, value, 1);
        }

        return data;
    }
}