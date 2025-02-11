
public static class ErrorMessage
{




    /// <summary>
    /// Displays an error message for invalid input.
    /// </summary>
    public static void DisplayInvalidInputMessage()
    {
        Console.SetCursorPosition((Console.WindowWidth - 40) / 2, Console.WindowHeight / 2 + 1);
        Console.Write("Invalid input. Press any key to continue...");
        Console.ReadKey(true);
    }






    /// <summary>
    /// Displays an error message for invalid grid size input.
    /// </summary>
    public static void DisplayInvalidSizeMessage()
    {
        Console.SetCursorPosition((Console.WindowWidth - 30) / 2, Console.WindowHeight / 2 + 1);
        Console.Write("Invalid size. Press any key to continue...");
        Console.ReadKey(true);
    }

}