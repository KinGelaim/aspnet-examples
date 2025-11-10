namespace Testing.XUnit;

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
        Assert.Equal(expectedResult, actualResult);
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
        Assert.Equal(expectedResult, actualResult);
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
        Assert.Equal(expectedResult, actualResult, 0.001);
    }

    [Fact]
    public void Division_ReturnsArgumentOutOfRangeException_WhenDenominatorIsZero()
    {
        // Arrange
        var numerator = 1;
        var denominator = 0;

        // Act and assert
        Assert.Throws<ArgumentOutOfRangeException>(() => Sut().Division(numerator, denominator));
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
        Assert.Equal(expectedResult, actualResult);
    }

    public static IEnumerable<object[]> GetEnumerableValues =>
    [
        [10, 5, 2],
        [-120, 2, -60]
    ];

    [Theory]
    //[MemberData(nameof(GetDivisionEnumerableData), parameters: [3, 1, 20])]
    [MemberData(nameof(GetDivisionTheoryData), parameters: [3, 1, 20])]
    public void Division_ReturnsDivisionResult_WithGeneratedValues(
        double numerator,
        double denominator,
        double expectedResult)
    {
        // Act
        var actualResult = Sut().Division(numerator, denominator);

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    public static IEnumerable<object[]> GetDivisionEnumerableData(
        int testsCount,
        int minLength,
        int maxLength)
    {
        var random = new Random();
        for (var i = 0; i < testsCount; i++)
        {
            var value = (double)random.Next(minLength, maxLength);

            yield return new object[] { value, value, 1 };
        }
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