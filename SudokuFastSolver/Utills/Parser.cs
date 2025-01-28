using System;

public class Parser
{
    private int[,] grid;
    private Settings settings;
    // Constructor that takes user input and parses it
    public Parser(Settings settings)
    {
        this.settings = settings;
        Console.WriteLine($"Enter the Sudoku puzzle as a single string of {settings.ExpectedLength} digits (0 for empty cells):");
        string input = Console.ReadLine();

        // Validate and parse the input
        grid = ParseStringToGrid(input);
    }

    // Function to convert a string to a 2D array
    private int[,] ParseStringToGrid(string input)
    {
        if (input.Length != settings.ExpectedLength)
        {
            throw new ArgumentException($"Input string must be exactly {settings.ExpectedLength} characters long.");
        }

        int[,] grid = new int[settings.GridSize, settings.GridSize];

        for (int row = 0; row < settings.GridSize; row++)
        {
            for (int col = 0; col < settings.GridSize; col++)
            {
                char c = input[row * 9 + col];
                if (c >= '0' && c <= '0' + settings.GridSize)
                {
                    grid[row, col] = c - '0'; // Convert char to int
                }
                else
                {
                    throw new ArgumentException("Input string must contain only digits (0-9).");
                }
            }
        }

        return grid;
    }

    // Property to access the parsed grid
    public int[,] Grid
    {
        get { return grid; }
    }
}