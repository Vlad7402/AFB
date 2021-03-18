using System;
using System.Collections.Generic;

namespace AFB.logic
{
    public class Expression
    {
        private readonly Files files;
        private readonly double velStart;
        private readonly double velEnd;
        private readonly double step;
        private readonly string[] input;
        private readonly string _RPNExpression;
        private readonly double[] _ValuemsOfX;
        private readonly RPN functionBilder;
        public Expression(IWriter writer)
        {
            files = new Files(writer);
            functionBilder = new RPN(writer);
            input = files.GetExpressionsFromFile();
            velStart = files.DoubleParser(files.DeletWhitespaces(input[1]));
            velEnd = files.DoubleParser(files.DeletWhitespaces(input[2]));
            step = files.DoubleParser(files.DeletWhitespaces(input[3]));
            _RPNExpression = GetRPNExpression();
            _ValuemsOfX = files.GetArgumentVels(velStart, velEnd, step);          
        }
        public string RPNExpression { get { return _RPNExpression; } }
        public double[] ValuemsOfX { get { return _ValuemsOfX; } }
        public double[] GetValuemsOfY()
        {
            List<double> velsOfY = new List<double>();
            for (int i = 0; i < _ValuemsOfX.Length; i++) velsOfY.Add(functionBilder.Counting(_RPNExpression, _ValuemsOfX[i]));
            return velsOfY.ToArray();
        }
        private string GetRPNExpression()
        {
            functionBilder.DoesEndExist(velStart, velEnd, step);
            string expression = files.DeletWhitespaces(input[0]);
            functionBilder.ExpressionCheck(expression);
            string[] toConvertInExpression = expression.Split(new char[] { 'y', '=' }, StringSplitOptions.RemoveEmptyEntries);
            expression = string.Empty;
            for (int i = 0; i < toConvertInExpression.Length; i++) expression += toConvertInExpression[i];
            string expressionInRPN = functionBilder.GetExpression("  " + expression);
            return expressionInRPN;
        }
    }
}
