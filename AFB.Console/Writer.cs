namespace AFB.Console
{
    using AFB.logic;
    using System;
    public class Writer : IWriter
    {
        public void PrintError(string errorText)
        {
            Console.Beep(500, 200);
            Console.WriteLine(errorText);
            Console.WriteLine("Для продолжения нажмите любую клавишу...");
            Console.ReadKey();
            Environment.Exit(0);
        }
        public void PrintTable(string[] table)
        {
            foreach (var line in table)
                Console.WriteLine(line);
        }
    }
}
