using AFB.logic;

namespace AFB.Console
{
    class Program
    {
        static void Main()
        {
            IWriter writer = new Writer();
            var expression = new Expression(writer);
            var files = new Files(writer);
            var table = Table.GetTable(expression.ValuemsOfX, expression.GetValuemsOfY());
            files.SaveTable(table);
            writer.PrintTable(table);
        }
    }
}
