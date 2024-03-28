internal class TextColorNotSetException : TextColorException
{
    private readonly string message = "text colors have have not been set for this instance";

    public override string Message => message;
}
