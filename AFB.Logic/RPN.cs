using System;
using System.Collections.Generic;

namespace AFB.logic
{
    //Да, возвращает строку, да, так нельзя, да, ОПЗ написана через одно место. Обязательно когда-нибудь переделаю :)
    public class RPN
    {
        private readonly IWriter writer;
        public RPN(IWriter writer)
        {
            this.writer = writer;
        }
        public string GetExpression(string input)
        {
            string result = "  ";
            Stack<char> operations = new Stack<char>();
            int prefixisStart = 0;
            int prefixSkip = 0;
            int prefixisEnd = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == 'x')
                {
                    if (IsPrefix(i, input))
                    {
                        result += '-';
                    }
                    result += "x ";
                }
                if (char.IsDigit(input[i]))
                {
                    if (IsPrefix(i, input))
                    {
                        result += '-';
                    }
                    while (!IsOperation(input[i]) || input[i] == '.')
                    {
                        result += input[i];
                        i++;
                        if (i == input.Length) break;
                    }
                    result += " ";
                    i--;
                }

                if (IsOperation(input[i]))
                {
                    if (input[i] == '(')
                    {
                        operations.Push(input[i]);
                        if (prefixisStart > 0)
                        {
                            result += "-1 ";
                            prefixisStart--;
                            prefixisEnd++;
                        }
                        else prefixSkip++;
                    }
                    else if (input[i] == ')')
                    {
                        string ejaktedOperation = operations.Pop().ToString();
                        while (ejaktedOperation != "(")
                        {
                            result += ejaktedOperation + ' ';
                            ejaktedOperation = operations.Pop().ToString();
                        }
                        if (prefixisEnd > 0 && prefixSkip == 0)
                        {
                            result += "* ";
                            prefixisEnd--;
                        }
                        else prefixSkip--;
                    }
                    else if (input[i] == '-')
                    {
                        if (input[i + 1] == '(') prefixisStart++;

                        else if (!IsPrefix(i + 1, input)) AddOperation(operations, ref result, i, input);
                    }
                    else
                    {
                        AddOperation(operations, ref result, i, input);
                    }
                }
            }
            while (operations.Count > 0)
                result += operations.Pop() + " ";
            return result;
        }
        public double Counting(string input, double velOfX)
        {
            Stack<double> nums = new Stack<double>();
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsWhiteSpace(input[i])) continue;

                if (input[i] == 'x')
                {
                    if (IsPrefix(i, input)) nums.Push(velOfX * -1);
                    else nums.Push(velOfX);

                }

                else if (char.IsDigit(input[i]))
                {
                    nums.Push(GetNumFromString(input, ref i));
                }
                else if (IsOperation(input[i]))
                {
                    if (input[i] == '-' && i + 1 != input.Length &&
                        input[i + 1] == 'x' || char.IsDigit(input[i + 1]))
                    {
                        i++;
                        nums.Push(GetNumFromString(input, ref i));
                    }
                    else
                    {
                        double firstNum = nums.Pop();
                        double secNum = 0;
                        if (!IsOperationUnar(input[i])) secNum = nums.Pop();

                        nums.Push(GetOperationResult(firstNum, secNum, input[i]));
                    }
                }
            }
            return nums.Peek();
        }
        private int GetPriority(char operation)
        {
            switch (operation)
            {
                case '+':
                case '-': return 0;
                case '*':
                case '/': return 1;
                case 's':
                case 'c':
                case 't':
                case 'l':
                case '!':
                case '^': return 2;
                default: return -1;
            }
        }
        private bool IsOperation(char operation)
        {
            return "+-/*^()!".IndexOf(operation) != -1 || char.IsLetter(operation) && operation != 'x';
        }
        private double GetOperationResult(double firstNum, double secNum, char operation)
        {
            double result = 0;
            switch (operation)
            {
                case '+': result = secNum + firstNum; break;
                case '-': result = secNum - firstNum; break;
                case '*': result = secNum * firstNum; break;
                case '/': if (IsDevisionAvaliable(firstNum)) result = secNum / firstNum; break;
                case 's': result = Math.Sin(firstNum * Math.PI / 180); break;
                case 'c': result = Math.Cos(firstNum * Math.PI / 180); break;
                case 't': result = Math.Tan(firstNum * Math.PI / 180); break;
                case 'l': result = Math.Log(firstNum); break;
                case '!': if (IsFactorialAvaliable(firstNum)) result = GetFactorial(firstNum); break;
                case '^': result = Math.Pow(secNum, firstNum); break;
            }
            return result;
        }
        private bool IsDevisionAvaliable(double val)
        {
            if (val != 0) return true;
            else
            {
                writer.PrintError("Ошибка: деление на 0");
                Environment.Exit(0);
                return false;
            }
        }
        private double GetFactorial(double val)
        {
            int result = 1;
            for (int i = 1; i <= (int)val; i++)
            {
                result *= i;
            }
            return result;
        }
        private bool IsFactorialAvaliable(double val)
        {
            if (val < 150) return true;
            else
            {
                writer.PrintError("Ошибка: превышение максималного значения для факториала(150) или значение равно 0");
                return false;
            }
        }
        private bool IsOperationUnar(char operation)
        {
            if ("sctl!".IndexOf(operation) != -1)
                return true;
            return false;
        }
        private void AddOperation(Stack<char> operations, ref string result, int inputSymbol, string input)
        {
            if (operations.Count > 0)
                if (GetPriority(input[inputSymbol]) <= GetPriority(operations.Peek()))
                    result += operations.Pop().ToString() + " ";
            operations.Push(char.Parse(input[inputSymbol].ToString()));
        }
        private static bool IsPrefix(int inputSymbol, string input)
        {
            return input[inputSymbol - 1] == '-' && (input[inputSymbol - 2] == ' ' || input[inputSymbol - 2] == '(');
        }
        private bool IsInUse(char symbol)
        {
            if ("+-/*^()sctl!0123456789.xy=".IndexOf(symbol) != -1)
                return true;
            return false;
        }
        public void ExpressionCheck(string input)
        {
            int openBracket = 0;
            int closeBracket = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(') openBracket++;
                if (input[i] == ')') closeBracket++;
                if (!IsInUse(input[i]))
                {
                    Console.Clear();
                    Console.WriteLine(input);
                    Console.SetCursorPosition(i, 1);
                    Console.WriteLine('^');
                    writer.PrintError("Ошибка: неизвестная операция в символе " + (i + 1));
                }
                if (i >= 1)
                    if (IsOperationCheking(input[i]) && IsOperationCheking(input[i - 1])) writer.PrintError("Ошибка: 2 бинарных оператора рядом");
            }
            if (openBracket != closeBracket) writer.PrintError("Ошибка: нарушение при постановке скобок");
        }
        public bool DoesEndExist(double velStart, double velEnd, double step)
        {
            if (velStart < velEnd && Math.Sign(step) == -1) writer.PrintError("Ошибка: количество значений X превышает 10000");
            else if (velStart > velEnd && Math.Sign(step) == 1) writer.PrintError("Ошибка: количество значений X превышает 10000");
            else if (Math.Sign(step) == 0) writer.PrintError("Ошибка: количество значений X превышает 10000");
            for (double i = velStart; i < velEnd; i++)
            {
                velStart += step;
                if (i > 10000) writer.PrintError("Ошибка: количество значений X превышает 10000");
            }
            return true;
        }
        private double GetNumFromString(string input, ref int ID)
        {
            string num = string.Empty;
            if (IsPrefix(ID, input)) num += '-';

            while (char.IsDigit(input[ID]) || input[ID] == '.')
            {
                num += input[ID];
                ID++;
                if (ID == input.Length) break;
            }
            num += " ";
            ID--;
            return double.Parse(num);
        }
        private static bool IsOperationCheking(char operation)
        {
            if (("+-/*^".IndexOf(operation) != -1))
                return true;
            return false;
        }
    }
}