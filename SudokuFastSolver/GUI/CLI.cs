using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;



/// <summary>
/// This is a CLI GUI part 
/// 
/// nice CLI GUI without any libs
/// 
/// 
/// took insperation from this git repo
/// by erictuvesson
/// https://github.com/erictuvesson/CSharpCurses.git
/// 
/// Check the code 
/// 
/// 
/// </summary>
public static class CLI
{
    // Board state
    private static char[,] _board; // char of the board
    private static int _boardSize = 9; // size  9 on start

    //dont clear the board if allready has solution
    private static bool _unInitilaized = true;



    //print the time if not null
    private static TimeTracker _time = null;


    // Show the unsolveable tag
    public static bool unsolveable = false;
    // IF not enterd via path   - so it doesnt write to file
    public static string? path = null;


    // UI elements
    private static int _selectedButton = 0;
    private static readonly string[] _buttons = { "Solve", "Clear", "Change Size", "Load Grid From File", "Load Grid", "Exit" };




    /// <summary>
    /// Initializes and starts the Sudoku GUI application.
    /// Handles the main application loop and window sizing requirements.
    /// </summary>
    public static void StartGUI()
    {
        Console.Title = "Sudoku Omega Solver";
        if (_unInitilaized) 
            InitializeBoard(_boardSize);


        Console.Clear();


        Render();
        while (true)
        {
            System.ConsoleKey key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Enter)
            {
                if (ExecuteButton())
                    return ;
            }
            else if (key == ConsoleKey.Tab)
            {
                _selectedButton = (_selectedButton + 1) % _buttons.Length;
            }
            else
            {
                //to not rerender no changes
                continue;
            }
            Console.Clear();
            Render();
        }

    }

    /// <summary>
    /// just activate the correct button 
    /// 
    /// 
    /// used switch case because im a good programer
    /// 
    /// 
    /// probebly could use hashmap for better preformence but its gui
    /// </summary>
    /// <returns></returns>
    private static bool ExecuteButton()
    {
        switch (_buttons[_selectedButton])
        {
            case "Solve":
                return true;
            case "Clear":
                InitializeBoard(_boardSize);
                break;
            case "Change Size":
                ChangeSize();
                break;
            case "Load Grid From File":
                LoadFromFile();
                break;
            case "Load Grid":
                LoadGrid();
                break;
            case "Exit":
                Environment.Exit(0);
                break;
        }
        return false;
    }
    /// <summary>
    /// Just run in a loop and set every value to 0
    /// 
    /// 
    /// </summary>
    /// <param name="size">The size of the board (4, 9, 16, or 25)</param>
    private static void InitializeBoard(int size)
    {
        path = null;
        _time = null;
        unsolveable = false;
        _unInitilaized = false;
        _boardSize = size;
        _board = new char[size, size];
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                _board[y, x] = '0';
    }



    /// <summary>
    /// Renders all interface elements including title, board, and buttons.
    /// </summary>
    private static void Render()
    {

        DrawTitle();
        DrawBoard();
        DrawButtons();
        PrintConectedFile();
        if (_time != null)
        {
            PrintTime();
        }
        if (unsolveable)
        {
            PrintUnsolveable();
        }
    }

    //--------------------------draw-title----------------------------------------------------------------


    /// <summary>
    /// Draws the ASCII art title at the top of the window.
    /// </summary>
    private static void DrawTitle()
    {
        string title = @"██    ██  ██████   █████  ██    ██     ███████ ██    ██ ██████   ██████  ██   ██ ██    ██ 
 ██  ██  ██    ██ ██   ██ ██    ██     ██      ██    ██ ██   ██ ██    ██ ██  ██  ██    ██ 
  ████   ██    ██ ███████ ██    ██     ███████ ██    ██ ██   ██ ██    ██ █████   ██    ██ 
   ██    ██    ██ ██   ██  ██  ██           ██ ██    ██ ██   ██ ██    ██ ██  ██  ██    ██ 
   ██     ██████  ██   ██   ████       ███████  ██████  ██████   ██████  ██   ██  ██████  
";
        Console.ForegroundColor = ConsoleColor.DarkGray;
        string[] titleLines = title.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        for (int i = 0; i < titleLines.Length; i++)
        {
            Console.SetCursorPosition((Console.WindowWidth - titleLines[i].Length) / 2, i + 1);
            Console.WriteLine(titleLines[i]);
        }
        Console.ResetColor();
    }


    //--------------------------------draw-----board-----------------------------------------------------------

    /// <summary>
    /// Draws the Sudoku board with grid lines and current values.
    /// 
    /// </summary>
    private static void DrawBoard()
    {
        int boxSize = (int)Math.Sqrt(_boardSize);

        int cellWidth = _boardSize <= 9 ? 3 : 2; // if bigger then 9 make cells smaller


        int cellHeight = 1;
        // pretty simple sizes
        int boardWidth = _boardSize * cellWidth + (_boardSize / boxSize - 1);
        int boardHeight = _boardSize * cellHeight + (_boardSize / boxSize - 1);
        int startX = (Console.WindowWidth - boardWidth) / 2;
        int startY = (Console.WindowHeight - boardHeight) / 2;

        for (int y = 0; y < _boardSize; y++)
        {
            DrawBoardRow(y, startX, startY, cellWidth, cellHeight, boxSize);
            DrawSectionDivider(y, startX, startY, boardWidth, boxSize, cellHeight);
        }
        Console.ResetColor();
    }

    /// <summary>
    /// Draws a single row of the Sudoku board.
    /// </summary>
    private static void DrawBoardRow(int y, int startX, int startY, int cellWidth, int cellHeight, int boxSize)
    {
        Console.SetCursorPosition(startX, startY + y * cellHeight + y / boxSize);

        for (int x = 0; x < _boardSize; x++)
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(FormatCellContent(x, y, cellWidth));
            Console.ResetColor();

            if (x < _boardSize - 1 && (x + 1) % boxSize == 0)
                Console.Write("║");
        }
    }


    /// <summary>
    /// Formats the content of a cell 
    /// 
    /// turns the zero to empty cells
    /// </summary>
    private static string FormatCellContent(int x, int y, int cellWidth)
    {
        if ( _board[y, x] == '0')
        {
            return new string(' ', cellWidth);
        }
        return _board[y, x].ToString().PadLeft(cellWidth - 1).PadRight(cellWidth);
    }

    /// <summary>
    /// Draws the horizontal divider between board sections.
    /// </summary>
    private static void DrawSectionDivider(int y, int startX, int startY, int boardWidth, int boxSize, int cellHeight)
    {
        if (y < _boardSize - 1 && (y + 1) % boxSize == 0)
        {
            Console.SetCursorPosition(startX, startY + (y + 1) * cellHeight + y / boxSize);
            Console.WriteLine(new string('═', boardWidth));
        }
    }

    //---------------end-board-drawing----------------------------------------------------------------------------------------


    /// <summary>
    /// Validates the input grid string.
    /// </summary>
    private static bool IsValidGridString(string input)
    {
        if (string.IsNullOrEmpty(input) || input.Length != _boardSize * _boardSize)
            return false;

        foreach (char c in input)
        {
            if (c == '0') continue;
            int value = c - '0';
            if (value < 1 || value > _boardSize)
                return false;
        }

        return true;
    }







    /// <summary>
    /// Loads the grid from a valid input string.
    /// </summary>
    public static void LoadGridFromString(string input)
    {
        int index = 0;
        for (int y = 0; y < _boardSize; y++)
        {
            for (int x = 0; x < _boardSize; x++)
            {
                _board[y, x] = input[index++];
            }
        }
    }



    /// <summary>
    /// Draws the interactive buttons at the bottom of the screen.
    /// </summary>
    private static void DrawButtons()
    {
        int totalButtonWidth = _buttons.Length * 10 + (_buttons.Length - 1) * 2;
        int startX = 0;
        int startY = Console.WindowHeight/15;

        Console.SetCursorPosition((Console.WindowWidth - 50) / 2, Console.WindowHeight - 3);

        Console.WriteLine("Press Tab to move the button and Enter to Select");
        for (int i = 0; i < _buttons.Length; i++)
        {
            DrawButton(i, startX, (i+1) * startY + 5 * startY);
        }
        Console.ResetColor();
    }



    /// <summary>
    /// Draws a single button with highlighting if selected.
    /// </summary>
    private static void DrawButton(int index, int startX, int startY)
    {
        int buttonWidth = _buttons[index].Length + 4;
        Console.SetCursorPosition(startX, startY);

        if (index == _selectedButton)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
        }
        else
        {
            Console.ResetColor();
        }

        Console.Write($" {index + 1}.{_buttons[index]} ");
    }



  



    /// <summary>
    /// Checks if the provided grid size is valid.
    /// </summary>
    private static bool IsValidGridSize(int size)
    {
        return Math.Sqrt(size)* Math.Sqrt(size) == size && size >= 4 && size <= 25;
    }





    /// <summary>
    /// Gets the board as a single string.
    /// not a ToString becuase its a static class
    /// and its imposible
    /// </summary>
    public static string GetBoardAsString()
    {
        StringBuilder sb = new StringBuilder(_boardSize * _boardSize);
        for (int y = 0; y < _boardSize; y++)
        {
            for (int x = 0; x < _boardSize; x++)
            {
                sb.Append(_board[y, x]);
            }
        }
        return sb.ToString();
    }

    //buttons ------------------------------------------------------------------------

    /// <summary>
    /// users to change the board size.
    /// </summary>
    private static void ChangeSize()
    {
        Console.Clear();

        DrawTitle();

        Console.SetCursorPosition((Console.WindowWidth - 20) / 2, Console.WindowHeight / 2);
        Console.Write("Enter grid size (4, 9, 16, 25): ");

        if (int.TryParse(Console.ReadLine(), out int size) && IsValidGridSize(size))
        {
            InitializeBoard(size);
        }
        else
        {
            ErrorMessage.DisplayInvalidSizeMessage();
        }
    }






    /// <summary>
    /// Handles loading a grid from a string input.
    /// </summary>
    private static void LoadGrid()
    {

        
        Console.Clear();
        DrawTitle();

        Console.SetCursorPosition((Console.WindowWidth - 50) / 2, Console.WindowHeight / 2);
        Console.Write($"Enter grid string ({_boardSize * _boardSize} characters): ");

        string input = Console.ReadLine();

        if (IsValidGridString(input))
        {
            LoadGridFromString(input);
            path = null;
            _time = null;
        }
        else
        {
            ErrorMessage.DisplayInvalidInputMessage();
        }
    }

    /// <summary>
    /// loads from file the board
    /// 
    /// 
    /// TODO : need to fix size of board
    /// only accsepts file in the size of cur  board
    /// </summary>
    public static void LoadFromFile()
    {

        Console.Clear();
        DrawTitle();

        Console.SetCursorPosition((Console.WindowWidth - 50) / 2, Console.WindowHeight / 2);
        
        Console.Write($"Enter Path To File (File string need to be {_boardSize * _boardSize}): ");
        

        // TODO : make this cleaner this is garbage code
        path = Console.ReadLine();
        try
        {
            string input = FileIO.ReadFromFile(path);
            if (IsValidGridString(input))
            {
                LoadGridFromString(input);
            }
            else
            {
                ErrorMessage.DisplayInvalidInputMessage();
                path = null;
                _time = null;
            }
        }
        catch
        {
            ErrorMessage.DisplayInvalidInputMessage();

        }

    }

    // not buttons anymore


    /// <summary>
    /// pretty simple function that sets the time and function that print it 
    /// i dont want to split this message ...
    /// </summary>
    /// <param name="time"></param>
    public static void SetTime(TimeTracker time)
    {
        _time = time;
    }
    public static void PrintTime()
    {

        Console.SetCursorPosition((Console.WindowWidth - 50) / 2, Console.WindowHeight - 4);

        _time.WriteTime();
    }


    /// <summary>
    /// pretty straight forward 
    /// </summary>
    public static void PrintConectedFile()
    {
        if(path != null)
        {
            Console.SetCursorPosition(Console.WindowWidth - 34 , Console.WindowHeight - 2);

            Console.Write("Write to File:");
            string last20 = path.Length > 20 ? path.Substring(path.Length - 20) : path; // takes the last 20 chars of the string

            Console.Write(last20);
        }
    }
    public static void PrintUnsolveable()
    {
        Console.SetCursorPosition((Console.WindowWidth - 42) / 2, Console.WindowHeight - 4);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("This Board Doesn't Have Any Solutions!!!");
        Console.ResetColor();

    }
}
