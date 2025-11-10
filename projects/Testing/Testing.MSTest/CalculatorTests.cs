using System.Reflection;

namespace Testing.MSTest;

[TestClass]
public sealed class CalculatorTests
{
    private Calculator Sut()
    {
        return new Calculator();
    }

    [TestMethod]
    public void Add_ReturnsSum()
    {
        // Arrange
        var firstNumber = 1;
        var secondNumber = 2;
        var expectedResult = 3;

        // Act
        var actualResult = Sut().Add(firstNumber, secondNumber);

        // Assert
        Assert.AreEqual(expectedResult, actualResult);
    }

    [DataTestMethod]
    [DataRow(4, 2, 2)]
    [DataRow(9, 3, 3)]
    [DataRow(9, -3, -3)]
    public void Division_ReturnsDivisionResult(double numerator, double denominator, double expectedResult)
    {
        // Act
        var actualResult = Sut().Division(numerator, denominator);

        // Assert
        Assert.AreEqual(expectedResult, actualResult);
    }

    [TestMethod]
    public void Division_ReturnsDivisionResult_WhenResultHasInaccuracy()
    {
        // Arrange
        var numerator = 10;
        var denominator = 3;
        var expectedResult = 3.333;

        // Act
        var actualResult = Sut().Division(numerator, denominator);

        // Assert
        Assert.AreEqual(expectedResult, actualResult, 0.001);
    }

    [TestMethod]
    public void Division_ReturnsArgumentOutOfRangeException_WhenDenominatorIsZero()
    {
        // Arrange
        var numerator = 1;
        var denominator = 0;

        // Act and assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Sut().Division(numerator, denominator));
    }

    [DataTestMethod]
    [DynamicData(nameof(GetEnumerableValues), DynamicDataSourceType.Property)]
    public void Division_ReturnsDivisionResult_WithEnumerableValues(
        double numerator,
        double denominator,
        double expectedResult)
    {
        // Act
        var actualResult = Sut().Division(numerator, denominator);

        // Assert
        Assert.AreEqual(expectedResult, actualResult);
    }

    private static IEnumerable<object[]> GetEnumerableValues =>
    [
        [10, 5, 2],
        [-120, 2, -60]
    ];

    [DataTestMethod]
    [CheckDivisionResult(3, 1, 20)]
    public void Division_ReturnsDivisionResult_WithGeneratedValues(
        double numerator,
        double denominator,
        double expectedResult)
    {
        // Act
        var actualResult = Sut().Division(numerator, denominator);

        // Assert
        Assert.AreEqual(expectedResult, actualResult);
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    private class CheckDivisionResultAttribute : Attribute, ITestDataSource
    {
        private readonly int _testsCount;
        private readonly int _minValue;
        private readonly int _maxValue;

        public CheckDivisionResultAttribute(int testsCount, int minLength, int maxLength)
        {
            _testsCount = testsCount;
            _minValue = minLength;
            _maxValue = maxLength;
        }

        public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
        {
            var random = new Random();
            for (var i = 0; i < _testsCount; i++)
            {
                var value = (double)random.Next(_minValue, _maxValue);

                yield return new object?[] { value, value, 1 };
            }
        }

        public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
        {
            if (data is null)
            {
                return methodInfo.Name;
            }

            var numeratorSuffix = data[0] is double numerator
                ? numerator.ToString()
                : "NULL";
            var denominatorSuffix = data[1] is double denominator
                ? denominator.ToString()
                : "NULL";

            return $"{methodInfo.Name}_Numerator{numeratorSuffix}_Denominator{denominatorSuffix}";
        }
    }
}