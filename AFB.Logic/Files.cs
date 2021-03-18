namespace AFB.logic
{
    using System;
    using System.IO;
    using System.Collections.Generic;

    public class Files
    {
        private readonly IWriter writer;
        public Files(IWriter writer)
        {
            this.writer = writer;
        }
        public string[] GetExpressionsFromFile()
        {
            string input = File.ReadAllText("input.txt");
            input = input.ToLower();
            if (IsInafCuters(input)) return input.Split(new char[] { '|' });
            else writer.PrintError("Ошибка: в файле отсутствуют все необходимые компoненты.");
            return null;
        }
        public void SaveExpression(double xStart, double xEnd, double xStep, string function)
        {
            var functionToSave = function + '|' + xStart.ToString() + '|' + xEnd.ToString() + '|' + xStep.ToString();
            File.WriteAllText("input.txt", functionToSave);
        }
        private bool IsInafCuters(string input)
        {
            int counter = 0;
            for (int i = 0; i < input.Length; i++) if (input[i] == '|') counter++;
            return counter == 3;
            //Через | подряд вводятся формула, X начальное, X конечное, шаг
        }
        public string DeletWhitespaces(string input)
        {
            string result = string.Empty;
            string[] splited = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var expression in splited)
                result += expression;

            return result;
        }
        public double DoubleParser(string input)
        {
            if (!double.TryParse(input, out double result)) writer.PrintError("Ошибка: невозможно преобразовать значение X начальное, X конечное или шаг в нужный формат.");
            return result;
        }
        public double[] GetArgumentVels(double velStart, double velEnd, double step)
        {
            List<double> result = new List<double>();
            result.Add(velStart);
            for (double i = velStart; i < velEnd; i++)
            {
                result.Add(velStart + step);
                velStart += step;
            }
            return result.ToArray();
        }
        public void SaveTable(string[] tableStrings)
        {
            File.WriteAllLines("output.txt", tableStrings);
        }
    }
}
