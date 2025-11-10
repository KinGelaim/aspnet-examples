using Testing;

var calculator = new Calculator();

var firstNumber = 1;
var secondNumber = 4;
var result = calculator.Add(firstNumber, secondNumber);

Console.WriteLine($"Сумма чисел {firstNumber} и {secondNumber} равна {result}");