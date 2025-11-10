namespace Testing.NUnit;

[TestFixture]
public class CalculatorTests
{
    private Calculator _calculator;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _calculator = new Calculator();
    }

    /*
    [SetUp]
    public void Setup() { }

    [OneTimeTearDown]
    public void OneTimeTearDown() { }

    [TearDown]
    public void TearDown() { }
    */

    [Test]
    public void Add_ReturnsSum()
    {
        // Arrange
        var firstNumber = 1;
        var secondNumber = 2;
        var expectedResult = 3;

        // Act
        var actualResult = _calculator.Add(firstNumber, secondNumber);

        // Assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [TestCase(4, 2, 2)]
    [TestCase(9, 3, 3)]
    [TestCase(9, -3, -3)]
    public void Division_ReturnsDivisionResult(double numerator, double denominator, double expectedResult)
    {
        // Act
        var actualResult = _calculator.Division(numerator, denominator);

        // Assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public void Division_ReturnsDivisionResult_WhenResultHasInaccuracy()
    {
        // Arrange
        var numerator = 10;
        var denominator = 3;
        var expectedResult = 3.333;

        // Act
        var actualResult = _calculator.Division(numerator, denominator);

        // Assert
        Assert.That(actualResult, Is.EqualTo(expectedResult).Within(0.001));
    }

    [Test]
    public void Division_ReturnsArgumentOutOfRangeException_WhenDenominatorIsZero()
    {
        // Arrange
        var numerator = 1;
        var denominator = 0;

        // Act and assert
        Assert.Throws<ArgumentOutOfRangeException>(() => _calculator.Division(numerator, denominator));
    }

    [TestCaseSource(nameof(GetEnumerableValues))]
    public void Division_ReturnsDivisionResult_WithEnumerableValues(
        double numerator,
        double denominator,
        double expectedResult)
    {
        // Act
        var actualResult = _calculator.Division(numerator, denominator);

        // Assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    private static IEnumerable<object[]> GetEnumerableValues =>
    [
        [10, 5, 2],
        [-120, 2, -60]
    ];

    [TestCaseSource(nameof(GetGeneratedValues))]
    public void Division_ReturnsDivisionResult_WithGeneratedValues(
        double numerator,
        double denominator,
        double expectedResult)
    {
        // Act
        var actualResult = _calculator.Division(numerator, denominator);

        // Assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    private static IEnumerable<object[]> GetGeneratedValues()
    {
        int testsCount = 5;
        int minValue = 1;
        int maxValue = 20;

        var random = new Random();
        for (var i = 0; i < testsCount; i++)
        {
            var value = (double)random.Next(minValue, maxValue);

            yield return new object[] { value, value, 1 };
        }
    }
}