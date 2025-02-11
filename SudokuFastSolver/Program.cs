using System;


/// <summary>
/// This is my sudoku solver 
/// 
/// 
/// here is an simple explantion of how it works
/// how its so fast 
/// in the solver file the explantion is deaper but this is a over view
/// 
/// 
/// first : get the input from the user pretty simple
/// 
/// 
/// second : inital check and activation of heavy heuristics 
/// (in this level for every cell saved the allowed digits to enter for every 
/// this way you cab activate hidden sets and naked sets)
/// 
/// third : you change how you save the what is allowed to enter board 
/// to lines colums and boxes
/// (this maked this faster to enter values check if they are valid and solve it also to save the board state a lot faster)
/// then simple heuristic backtracking with hidden single naked single 
/// 
/// this solves most 9x9 sudokus in under 2 ms
/// most 16x16 in under 50 ms
/// and most 25x25 in under 100 ms 
/// 
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("press 1 for input via cli and press 2 for file input");


            // just for test need to change this
            // TODO : fix ui make this look better
            // this code is garbage
            System.ConsoleKey key = Console.ReadKey(true).Key;
            string input = null;
            bool EnterFile = false;
            string path = null;

            if (key == ConsoleKey.D1)
            {
                Console.WriteLine("Enter Sudoku puzzle (0 for empty cells, no spaces):");
                input = Console.ReadLine();
            }
            else if (key == ConsoleKey.D2)
            {
                Console.WriteLine("Enter full path to file");
                path = Console.ReadLine();
                input = FileIO.ReadFromFile(path);
                EnterFile = true;

            }

            Board board = new Board();
            try
            {
                board.LoadPuzzle(input);
                Console.WriteLine("\nInput puzzle:");
                board.Print();
                Solver solver = new Solver(board);
                Console.WriteLine("\nSolving...");
                TimeTracker timer = new TimeTracker();

                timer.Start();
                bool solved = solver.Solve();
                timer.Stop();
                

                if (solved)
                {
                    Console.WriteLine("\nSolution:");
                    board.Print();
                    Console.WriteLine(board);
                    timer.PrintTime();
                }
                else
                {
                    Console.WriteLine("No solution exists!");
                    timer.PrintTime();
                }
                if (EnterFile)
                {
                    FileIO.WriteToFile(path, board.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}