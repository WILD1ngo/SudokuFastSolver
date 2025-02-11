using System;
using System.IO;


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


    static string? path = null;
    public static void Main(string[] args)
    {
        while (true)
        {
            
            //turn on or off the gui
            bool GUI = true; 
            try
            { 

                //its get the input via GUI or CLI
                string? input = getInput(GUI); // gets the input from the user

                //initlaize the board
                Board board = new Board();
                board.LoadPuzzle(input);


                //get a progress update if GUI is not working 
                if (!GUI)
                {
                    Console.WriteLine("\nInput puzzle:");
                    board.Print();
                }


                // create the solver
                Solver solver = new Solver(board);
                //create the Timetracer
                TimeTracker timer = new TimeTracker();


                //solve the board and calc time
                timer.Start();
                bool solved = solver.Solve();
                timer.Stop();


                //enter the solution to board
                printSolution(GUI, solved, board, timer);



                //if enter via file write answer to the file
                if (path != null)
                {
                    FileIO.WriteToFile(path, board.ToString());
                    path = null;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }




    /// <summary>
    /// a function that get input from the user 
    /// 
    /// 
    /// if gui is on it uses this
    /// 
    /// 
    /// else uses normal simple stuff
    /// </summary>
    /// <param name="GUI"></param>
    /// <returns></returns>
    public static string? getInput(bool GUI)
    {
        
        if (GUI)
        {
            
            CLI.StartGUI();
            return CLI.GetBoardAsString();
        }
        else
        {
            
            Console.WriteLine("press 1 for input in cli and press 2 from file");
            System.ConsoleKey key = Console.ReadKey(true).Key;


            if (key == ConsoleKey.D1)
            {
                Console.WriteLine("Enter Sudoku puzzle (0 for empty cells, no spaces):");
                return Console.ReadLine();
            }
            else if (key == ConsoleKey.D2)
            {
                Console.WriteLine("Enter full path to file");
                path = Console.ReadLine();
                return FileIO.ReadFromFile(path);
            }
            else
            {
                Console.WriteLine("Press allowed values");
                return null;
            }


        }
        return null;
    }


    /// <summary>
    /// 
    /// print the solution 
    /// 
    /// if gui is on
    /// its enters it the gui
    /// 
    /// </summary>
    /// <param name="GUI"></param>
    /// <param name="solved"></param>
    /// <param name="solvedBoard"></param>
    /// <param name="timer"></param>
    public static void printSolution(bool GUI , bool solved , Board solvedBoard, TimeTracker timer)
    {
        if (solved)
        {
            if (!GUI)
            {
                Console.WriteLine("\nSolution:");
                solvedBoard.Print();
                timer.PrintTime();
            }
            else
            {
                CLI.LoadGridFromString(solvedBoard.ToString());
                CLI.SetTime(timer);
                path = CLI.path;
            }
        }
        else
        {
            if (!GUI)
            {
                Console.WriteLine("No solution exists!");
                timer.PrintTime();
            }
            else
            {
                CLI.unsolveable = true;
                CLI.SetTime(timer);
            }

        }
    }
}