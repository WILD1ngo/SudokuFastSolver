public class Program
{
    public static void Main()
    {
        try
        {

            //Change the settings here
            //
            //
            var settings = new Settings(
            gridSize: 9,  // The size of the grid
            showEmptySquare: false, // Show empty squares 
            emptyCell: "0", // How to represent empty squares
            showGridLines: true // Show grid lines
            );



            //Initialize the board and get the input from the user
            var board = new Board(settings);

            //Print the board
            board.Print();
        }
        catch (Exception ex)
        {
            //Handle exceptions
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}