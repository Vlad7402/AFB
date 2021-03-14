namespace AFB.logic
{
    public interface IWriter
    {
        void PrintError(string errorText);
        public void PrintTable(string[] table);
    }
}
