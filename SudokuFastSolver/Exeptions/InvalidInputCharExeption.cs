public class InvalidInputCharExeption : Exception
{
    public InvalidInputCharExeption()
        : base($"Invalid input charcters. Only numbers are allowed to enter in the input")
    {
    }
}
