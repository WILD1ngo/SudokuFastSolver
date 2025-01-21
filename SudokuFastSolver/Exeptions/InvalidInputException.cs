

public class InvalidInputException : Exception
{
    public InvalidInputException(int expected, int received)
        : base($"Invalid input length. Expected {expected} digits, got {received}")
    {
    }
}
