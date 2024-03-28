internal class TextColorException : Exception
{
    private readonly string message = "text colors have to be of Dictionary<string, ConsoleColor> type";

    public override string Message => message;
}
