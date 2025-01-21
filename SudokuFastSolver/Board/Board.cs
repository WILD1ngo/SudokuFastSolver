using System.Runtime.CompilerServices;

public class Board
{
    private readonly int[,] grid;
    public readonly Settings settings;
    public Box[] boxes;
    public Line[] rows;
    public Line[] cols;
    public Board(Settings settings , bool getUserInput = true)
    {

            
        this.settings = settings;
        
        boxes = new Box[settings.GridSize];
        rows = new Line[settings.GridSize];
        cols = new Line[settings.GridSize];

        //Initialize the grid as a 2D array
        grid = new int[settings.GridSize, settings.GridSize];

        //Get the input from the user
        if (getUserInput)
            ReadInput();
    }


    private void ReadInput()
    {

        Console.WriteLine($"Enter {settings.GridSize * settings.GridSize} digits (0 for empty cells):");
        string input = Console.ReadLine() ?? string.Empty;

        CheckInput(input);
        for (int i = 0; i < settings.GridSize; i++)
        {
            boxes[i] = new Box(settings.GridSize);
            rows[i] = new Line(settings.GridSize);
            cols[i] = new Line(settings.GridSize);
        }

        //Intilate the grid with the input
        for (int row = 0; row < settings.GridSize; row++)
        {
            for (int col = 0; col < settings.GridSize; col++)
            {
                int num = input[row * settings.GridSize + col] - '0';
                grid[row, col] = num;
                
                if (num != 0 && (rows[row].Contains[num - 1] || cols[col].Contains[num - 1] || boxes[row / settings.BoxSize * settings.BoxSize + col / settings.BoxSize].Contains[num - 1]))
                    throw new Exception();//TODO: chagne to custom exeption
                rows[row].Set(num);
                cols[col].Set(num);
                boxes[row / settings.BoxSize * settings.BoxSize + col / settings.BoxSize].Set(num);
            }
        }
    }

    //Prints the board
    public void Print()
    {
        var line = settings.ShowGridLines ? new string('-', settings.GridSize * 3 + settings.BoxSize + 1) : "";

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
        if (input.Any(c => (c - '0') > settings.GridSize))
        { 
            //TODO: change the exeption type
            throw new InvalidInputCharExeption();
        }

        if (input.Length != settings.ExpectedLength)
        {
            throw new InvalidInputException(settings.ExpectedLength, input.Length);
        }
    }

    public int GetValue(int row, int col) => grid[row, col];
    public void SetValue(int row, int col, int value) {
        rows[row].Set(value , grid[row, col]);
        cols[col].Set(value , grid[row, col]);
        boxes[row / settings.BoxSize * settings.BoxSize + col / settings.BoxSize].Set(value , grid[row, col]);
        grid[row, col] = value;
        
        
    }
    public bool IsEmpty(int row, int col) => grid[row, col] == 0;
    public int Size => settings.GridSize;
    public Board Clone()
    {
        // Create a new board with the same settings
        var clonedBoard = new Board(settings , false);

        // Deep copy the grid
        for (int row = 0; row < settings.GridSize; row++)
        {
            for (int col = 0; col < settings.GridSize; col++)
            {
                clonedBoard.grid[row, col] = this.grid[row, col];
            }
        }

        // Deep copy rows, cols, and boxes
        for (int i = 0; i < settings.GridSize; i++)
        {
            clonedBoard.rows[i] = this.rows[i].Clone();
            clonedBoard.cols[i] = this.cols[i].Clone();
            clonedBoard.boxes[i] = this.boxes[i].Clone();
        }

        return clonedBoard;
    }

}