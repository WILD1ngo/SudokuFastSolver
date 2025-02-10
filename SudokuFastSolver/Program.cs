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
/// and  most 25x25 in under 100 ms 
/// 
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Enter Sudoku puzzle (0 for empty cells, no spaces):");
            string input = Console.ReadLine();

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
                    timer.PrintTime();
                }
                else
                {
                    Console.WriteLine("No solution exists!");
                    timer.PrintTime();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}