public class Board
{
    private readonly int[,] grid;
    private readonly Settings settings;

    public Board(Settings settings)
    {

            
        this.settings = settings;


        //Initialize the grid as a 2D array
        grid = new int[settings.GridSize, settings.GridSize];

        //Get the input from the user
        ReadInput();
    }


    private void ReadInput()
    {

        Console.WriteLine($"Enter {settings.GridSize * settings.GridSize} digits (0 for empty cells):");
        string input = Console.ReadLine() ?? string.Empty;

        CheckInput(input);

        //Intilate the grid with the input
        for (int i = 0; i < settings.GridSize; i++)
        {
            for (int j = 0; j < settings.GridSize; j++)
            {
                grid[i, j] = input[i * settings.GridSize + j] - '0';
            }
        }
    }

    //Prints the board
    public void Print()
    {
        var line = settings.ShowGridLines ? new string('-', settings.GridSize * 4 + 1) : "";

        for (int i = 0; i < settings.GridSize; i++)
        {

            if (settings.ShowGridLines)
            {
                if (i % settings.BoxSize == 0) Console.WriteLine(line);
                Console.Write("|");
            }

            for (int j = 0; j < settings.GridSize; j++)
            {
                if (grid[i, j] == 0)
                    Console.Write(settings.ShowEmptySquare ? $" {settings.EmptyCell} " : "   ");
                else
                    Console.Write($" {grid[i, j]} ");

                if (settings.ShowGridLines && (j + 1) % settings.BoxSize == 0)
                    Console.Write("|");
            }
            Console.WriteLine();
        }

        if (settings.ShowGridLines) Console.WriteLine(line);
    }
    private void CheckInput(string input) {
        if (!input.All(char.IsDigit))
        {
            throw new InvalidInputCharExeption();
        }

        if (input.Length != settings.ExpectedLength)
        {
            throw new InvalidInputException(settings.ExpectedLength, input.Length);
        }
    }

    public int GetValue(int row, int col) => grid[row, col];
    public void SetValue(int row, int col, int value) => grid[row, col] = value;
    public bool IsEmpty(int row, int col) => grid[row, col] == 0;
    public int Size => settings.GridSize;
}