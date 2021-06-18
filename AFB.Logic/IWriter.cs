namespace AFB.logic
{
    public interface IWriter
    {
        void PrintError(string errorText);
        void PrintTable(string[] table);
    }
}
