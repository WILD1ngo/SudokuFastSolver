using System.Runtime.InteropServices;

public class Program
{
    public static void Main()
    {
        try
        {

            //Change the settings 
            var settings = new Settings(
            gridSize: 9,  // The size of the grid
            showEmptySquare: false, // Show empty squares 
            emptyCell: "0", // How to represent empty squares
            showGridLines: true // Show grid lines
            );

            // Create a SudokuParser object
            Parser parser = new Parser(settings);
            


            //Initialize the board and get the input from the user
            var board = new Board(parser.Grid, settings);

            //Print the board
            board.Print();
            //solve the board
            var solver = new Solver(board);

            //Print the solution
            solver.PrintSolution();

        }
        catch (Exception ex)
        {
            //Handle exceptions
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}