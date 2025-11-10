namespace Testing;

internal sealed class Calculator
{
    public double Add(double firstNumber, double secondNumber)
    {
        return firstNumber + secondNumber;
    }

    public double Division(double numerator, double denominator)
    {
        ArgumentOutOfRangeException.ThrowIfZero(denominator);

        return numerator / denominator;
    }
}